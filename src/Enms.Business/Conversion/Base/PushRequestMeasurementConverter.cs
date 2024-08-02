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

  public abstract Task<IEnumerable<IMeasurement>> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request);

  public HttpContent ToHttpContent(IEnumerable<IMeasurement> measurement)
  {
    return ToHttpContent(measurement.Cast<TMeasurement>());
  }

  protected abstract HttpContent ToHttpContent(
    IEnumerable<TMeasurement> measurement);
}
