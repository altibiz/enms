using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IPushRequestMeasurementConverter
{
  bool CanConvert(string meterId);

  IEnumerable<IMeasurement> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request);

  Stream ToPushRequest(IEnumerable<IMeasurement> measurement);

  HttpContent ToHttpContent(IEnumerable<IMeasurement> measurement);
}
