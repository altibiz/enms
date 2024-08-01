using System.Text.Json.Nodes;
using Enms.Business.Conversion.Base;
using Enms.Business.Models;

namespace Enms.Business.Conversion;

public sealed class
  EgaugePushRequestMeasurementConverter : PushRequestMeasurementConverter<
  EgaugeMeasurementModel>
{
  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  public override IEnumerable<EgaugeMeasurementModel> ToMeasurements(
    string meterId,
    JsonNode request,
    DateTimeOffset timestamp)
  {
    throw new NotImplementedException();
  }

  protected override JsonNode ToPushRequest(
    IEnumerable<EgaugeMeasurementModel> measurement)
  {
    throw new NotImplementedException();
  }
}
