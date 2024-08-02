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
    var measurements = await pushRequestMeasurementConverter.ToMeasurements(
      meterId,
      DateTimeOffset.UtcNow,
      request
    );

    await batchAggregatedMeasurementUpserter
      .BatchAggregatedUpsert(measurements);
  }
}
