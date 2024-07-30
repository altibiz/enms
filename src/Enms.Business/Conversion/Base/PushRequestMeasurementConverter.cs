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

  public abstract IEnumerable<IMeasurement> ToMeasurements(
    string meterId,
    XDocument request,
    DateTimeOffset timestamp);

  public XDocument ToPushRequest(IEnumerable<IMeasurement> measurement)
  {
    return ToPushRequest(measurement.Cast<TMeasurement>());
  }

  protected abstract XDocument ToPushRequest(
    IEnumerable<TMeasurement> measurement);
}
