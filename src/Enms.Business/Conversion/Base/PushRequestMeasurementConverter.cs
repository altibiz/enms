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
    return meterId.StartsWith(MeterIdPrefix, StringComparison.InvariantCulture);
  }

  public abstract Task<IEnumerable<IMeasurement>> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request);

  public HttpContent ToHttpContent(IEnumerable<IMeasurement> measurements)
  {
    return ToHttpContent(measurements.Cast<TMeasurement>());
  }

  protected abstract HttpContent ToHttpContent(
    IEnumerable<TMeasurement> measurements);
}
