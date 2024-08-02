using Enms.Business.Models.Abstractions;
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
  RepeatingCsvResourceMeasurementGenerator<TMeasurementRecord>(
    IServiceProvider serviceProvider) : IMeasurementGenerator
  where TMeasurementRecord : class, IMeasurementRecord
{
  private readonly AgnosticMeasurementRecordMeasurementConverter _converter =
    serviceProvider
      .GetRequiredService<AgnosticMeasurementRecordMeasurementConverter>();

  private readonly AgnosticCumulativeCorrector _corrector =
    serviceProvider.GetRequiredService<AgnosticCumulativeCorrector>();

  private readonly ResourceCache _resources =
    serviceProvider.GetRequiredService<ResourceCache>();

  protected abstract string CsvResourceName { get; }

  protected abstract string LineIdPrefix { get; }

  public bool CanGenerateMeasurementsFor(string meterId)
  {
    return meterId.StartsWith(LineIdPrefix);
  }

  public async Task<List<IMeasurement>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId,
    string lineId,
    CancellationToken cancellationToken = default
  )
  {
    var records = await _resources
      .GetAsync<CsvLoader<TMeasurementRecord>, List<TMeasurementRecord>>(
        CsvResourceName,
        cancellationToken);
    var pushRequestMeasurements =
      ExpandRecords(records, meterId, lineId, dateFrom, dateTo).ToList();
    return pushRequestMeasurements;
  }

  private IEnumerable<IMeasurement> ExpandRecords(
    List<TMeasurementRecord> records,
    string meterId,
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

    var firstMeasurement = _converter.ConvertToMeasurement(firstRecord);
    var lastMeasurement = _converter.ConvertToMeasurement(lastRecord);

    var csvRecordsMinTimestamp = firstMeasurement.Timestamp;
    var csvRecordsMaxTimestamp = lastMeasurement.Timestamp;
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
        var measurement = _converter.ConvertToMeasurement(record);
        var corrected = _corrector.Correct(
          timestamp,
          meterId,
          lineId,
          measurement,
          firstMeasurement,
          lastMeasurement
        );
        yield return corrected;
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
