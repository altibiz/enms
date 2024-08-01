using System.Text.Json.Nodes;
using Enms.Business.Aggregation;
using Enms.Business.Conversion.Agnostic;

namespace Enms.Business.Iot;

public class EnmsIotHandler(
  AgnosticPushRequestMeasurementConverter pushRequestMeasurementConverter,
  BatchAggregatedMeasurementUpserter batchAggregatedMeasurementUpserter
)
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable IDE0060 // Remove unused parameter
  public async Task<bool> Authorize(string meterId, JsonNode request)
  {
    return true;
  }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

  public async Task OnPush(string meterId, JsonNode request)
  {
    var measurements = pushRequestMeasurementConverter.ToMeasurements(
      meterId,
      request,
      DateTimeOffset.UtcNow
    );

    await batchAggregatedMeasurementUpserter
      .BatchAggregatedUpsert(measurements);
  }
}
