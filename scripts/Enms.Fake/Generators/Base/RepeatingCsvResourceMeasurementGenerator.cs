using System.Xml.Linq;
using Enms.Business.Iot;
using Enms.Fake.Conversion.Agnostic;
using Enms.Fake.Correction.Agnostic;
using Enms.Fake.Generators.Abstractions;
using Enms.Fake.Loaders;
using Enms.Fake.Records.Abstractions;

// TODO: check time logic here
// TODO: add energy logic
// TODO: fixup cumulatives
// TODO: repeating enumerable

namespace Enms.Fake.Generators.Base;

public abstract class
  RepeatingCsvResourceMeasurementGenerator<TMeasurement>(
    IServiceProvider serviceProvider) : IMeasurementGenerator
  where TMeasurement : class, IMeasurementRecord
{
  private readonly AgnosticMeasurementRecordPushRequestConverter _converter =
    serviceProvider
      .GetRequiredService<AgnosticMeasurementRecordPushRequestConverter>();

  private readonly AgnosticCumulativeCorrector _corrector =
    serviceProvider.GetRequiredService<AgnosticCumulativeCorrector>();

  private readonly ResourceCache _resources =
    serviceProvider.GetRequiredService<ResourceCache>();

  protected abstract string CsvResourceName { get; }

  protected abstract string LineIdPrefix { get; }

  public bool CanGenerateMeasurementsFor(string lineId)
  {
    return lineId.StartsWith(LineIdPrefix);
  }

  public async Task<List<XDocument>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string lineId,
    CancellationToken cancellationToken = default
  )
  {
    var records = await _resources
      .GetAsync<CsvLoader<TMeasurement>, List<TMeasurement>>(
        CsvResourceName,
        cancellationToken);
    var pushRequestMeasurements =
      ExpandRecords(records, lineId, dateFrom, dateTo).ToList();
    return pushRequestMeasurements;
  }

  private IEnumerable<XDocument> ExpandRecords(
    List<TMeasurement> records,
    string lineId,
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo
  )
  {
    var ordered = records.OrderBy(record => record.Timestamp).ToList();
    var firstRecord = ordered.FirstOrDefault();
    var lastRecord = ordered.LastOrDefault();
    if (firstRecord == null || lastRecord == null)
    {
      yield break;
    }

    var csvRecordsMinTimestamp = firstRecord.Timestamp;
    var csvRecordsMaxTimestamp = lastRecord.Timestamp;
    var csvRecordsTimeSpan = csvRecordsMaxTimestamp - csvRecordsMinTimestamp;

    var timeSpan = dateTo - dateFrom;
    var dateFromCsv = csvRecordsMinTimestamp.AddTicks(
      (dateFrom - csvRecordsMinTimestamp).Ticks % csvRecordsTimeSpan.Ticks
    );
    var dateToCsv = dateFromCsv + timeSpan > csvRecordsMaxTimestamp
      ? csvRecordsMaxTimestamp
      : dateFromCsv + timeSpan;
    var currentDateFrom = dateFrom;
    var currentDateTo = dateFrom + (dateToCsv - dateFromCsv);
    while (timeSpan > TimeSpan.Zero)
    {
      foreach (var record in ordered
        .Where(
          record =>
            record.Timestamp >= dateFromCsv
            && record.Timestamp < dateToCsv))
      {
        var timestamp = currentDateFrom.AddTicks(
          (record.Timestamp - dateFromCsv).Ticks
        );
        var withCorrectedCumulatives = _corrector.CorrectCumulatives(
          timestamp,
          record,
          firstRecord,
          lastRecord
        );
        var json = _converter.ConvertToPushRequest(withCorrectedCumulatives);
        yield return new MessengerPushRequestMeasurement(
          lineId,
          timestamp,
          json
        );
      }

      timeSpan -= dateToCsv - dateFromCsv;

      dateFromCsv = dateToCsv == csvRecordsMaxTimestamp
        ? csvRecordsMinTimestamp
        : dateToCsv;
      dateToCsv = dateFromCsv + timeSpan > csvRecordsMaxTimestamp
        ? csvRecordsMaxTimestamp
        : dateFromCsv + timeSpan;

      currentDateFrom = currentDateTo;
      currentDateTo = currentDateFrom + (dateToCsv - dateFromCsv);
    }
  }
}
