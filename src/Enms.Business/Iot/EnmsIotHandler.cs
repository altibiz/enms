using System.Xml.Linq;
using Enms.Business.Aggregation;
using Enms.Business.Conversion.Agnostic;

namespace Enms.Business.Iot;

public class EnmsIotHandler(
    AgnosticPushRequestMeasurementConverter pushRequestMeasurementConverter,
    BatchAggregatedMeasurementUpserter batchAggregatedMeasurementUpserter
  )
{
  public async Task OnPush(string meterId, string request)
  {
    var measurements = pushRequestMeasurementConverter.ToMeasurements(
      meterId,
      XDocument.Parse(request),
      DateTimeOffset.UtcNow
    );

    await batchAggregatedMeasurementUpserter.BatchAggregatedUpsert(measurements);
  }
}
