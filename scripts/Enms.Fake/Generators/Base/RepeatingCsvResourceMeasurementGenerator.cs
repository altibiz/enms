using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Data.Entities.Abstractions;
using Enms.Fake.Generators.Abstractions;
using Enms.Fake.Loaders;

// TODO: check time logic here
// TODO: fixup cumulatives

namespace Enms.Fake.Generators.Base;

public abstract class
  RepeatingCsvResourceMeasurementGenerator<TMeasurement> : IMeasurementGenerator
  where TMeasurement : IMeasurementEntity
{
  private readonly ResourceCache _resources;

  private readonly IServiceProvider _serviceProvider;

  public RepeatingCsvResourceMeasurementGenerator(
    IServiceProvider serviceProvider)
  {
    _resources = serviceProvider.GetRequiredService<ResourceCache>();
    _serviceProvider = serviceProvider;
  }

  protected abstract string CsvResourceName { get; }

  protected abstract string MeterIdPrefix { get; }

  public bool CanGenerateMeasurementsFor(string meterId)
  {
    return meterId.StartsWith(MeterIdPrefix);
  }

  public async Task<List<string>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId
  )
  {
    var records = await _resources
      .GetAsync<CsvLoader<TMeasurement>, List<TMeasurement>>(CsvResourceName);
    var csvRecordsMinTimestamp = records.Min(record => record.Timestamp);
    var csvRecordsMaxTimestamp = records.Max(record => record.Timestamp);
    var csvRecordsTimeSpan = csvRecordsMaxTimestamp - csvRecordsMinTimestamp;
    dateFrom = csvRecordsMinTimestamp.AddTicks(
      (dateFrom - csvRecordsMinTimestamp).Ticks % csvRecordsTimeSpan.Ticks
    );
    dateTo = csvRecordsMinTimestamp.AddTicks(
      (dateTo - csvRecordsMinTimestamp).Ticks % csvRecordsTimeSpan.Ticks
    );
    return records
      .Where(record =>
        record.Timestamp >= dateFrom
        && record.Timestamp <= dateTo)
      .Select(entity => _serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(c => c.CanConvertToModel(entity.GetType()))
        ?.ToModel(entity))
      .OfType<IMeasurement>()
      .Select(measurement =>
      {
        var converter = _serviceProvider
          .GetServices<IPushRequestMeasurementConverter>()
          .FirstOrDefault(c => c.CanConvert(measurement.MeterId));
        if (converter is null)
        {
          return null;
        }

        var timestamp = csvRecordsMinTimestamp.AddTicks(
          (measurement.Timestamp - csvRecordsMinTimestamp).Ticks %
          csvRecordsTimeSpan.Ticks
        );

        return converter.ToPushRequest(measurement);
      })
      .OfType<string>()
      .ToList();
  }
}
