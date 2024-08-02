using System.Text.Json;
using Enms.Business.Aggregation;
using Enms.Business.Conversion.Agnostic;

namespace Enms.Business.Iot;

public class EnmsIotHandler(
  AgnosticPushRequestMeasurementConverter pushRequestMeasurementConverter,
  BatchAggregatedMeasurementUpserter batchAggregatedMeasurementUpserter
)
{
  public async Task OnPush(string meterId, Stream request)
  {
    var measurements = pushRequestMeasurementConverter.ToMeasurements(
      meterId,
      DateTimeOffset.UtcNow,
      request
    );

    await batchAggregatedMeasurementUpserter
      .BatchAggregatedUpsert(measurements);
  }
}

internal static class PushRequestMeasurementConverterOptions
{
  public static readonly JsonSerializerOptions Options =
    new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
