using System.Text.Json.Nodes;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Agnostic;

public class AgnosticMeasurementRecordPushRequestConverter(
  IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public JsonObject ConvertToPushRequest(IMeasurementRecord record)
  {
    var converter = _serviceProvider
      .GetServices<IMeasurementRecordPushRequestConverter>()
      .FirstOrDefault(c => c.CanConvertToPushRequest(record));

    return converter?.ConvertToPushRequest(record)
      ?? throw new InvalidOperationException(
        $"No converter found for {record.GetType()}");
  }
}
