using Enms.Business.Models.Abstractions;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Agnostic;

public class AgnosticMeasurementRecordMeasurementConverter(
  IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public IMeasurement ConvertToMeasurement(
    IMeasurementRecord record)
  {
    var converter = _serviceProvider
      .GetServices<IMeasurementRecordMeasurementConverter>()
      .FirstOrDefault(c => c.CanConvertToMeasurement(record.MeterId));

    return converter?.ConvertToMeasurement(record)
      ?? throw new InvalidOperationException(
        $"No converter found for {record.GetType()}");
  }
}
