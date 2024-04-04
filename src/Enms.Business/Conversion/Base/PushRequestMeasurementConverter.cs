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

  public abstract IMeasurement ToMeasurement(string pushRequest, string meterId,
    DateTimeOffset timestamp);

  public string ToPushRequest(IMeasurement measurement)
  {
    return ToPushRequest(measurement as TMeasurement
                         ?? throw new ArgumentException(
                           "Measurement is not of the expected type."));
  }

  protected abstract string ToPushRequest(TMeasurement measurement);
}
