using System.Xml.Linq;
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

  public abstract IMeasurement ToMeasurement(
    XDocument request,
    string meterId,
    DateTimeOffset timestamp);

  public XDocument ToPushRequest(IMeasurement measurement)
  {
    return ToPushRequest(
      measurement as TMeasurement
      ?? throw new ArgumentException(
        "Measurement is not of the expected type."));
  }

  protected abstract XDocument ToPushRequest(TMeasurement measurement);
}
