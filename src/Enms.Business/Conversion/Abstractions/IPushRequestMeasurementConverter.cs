using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IPushRequestMeasurementConverter
{
  bool CanConvert(string meterId);

  Task<IEnumerable<IMeasurement>> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request);

  HttpContent ToHttpContent(IEnumerable<IMeasurement> measurement);
}
