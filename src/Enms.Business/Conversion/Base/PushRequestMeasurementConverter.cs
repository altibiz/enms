using System.Text.Json.Nodes;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Base;

public abstract class
  PushRequestMeasurementConverter<TMeasurement> :
  IPushRequestMeasurementConverter
  where TMeasurement : class, IMeasurement
{
  protected abstract string MeterIdPrefix { get; }

  public bool CanConvert(string meterId)
  {
    return meterId.StartsWith(MeterIdPrefix);
  }

  public abstract IEnumerable<IMeasurement> ToMeasurements(
    string meterId,
    JsonNode request,
    DateTimeOffset timestamp);

  public JsonNode ToPushRequest(IEnumerable<IMeasurement> measurement)
  {
    return ToPushRequest(measurement.Cast<TMeasurement>());
  }

  protected abstract JsonNode ToPushRequest(
    IEnumerable<TMeasurement> measurement);
}
