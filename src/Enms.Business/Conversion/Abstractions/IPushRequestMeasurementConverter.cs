using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IPushRequestMeasurementConverter
{
  bool CanConvert(string meterId);

  IMeasurement ToMeasurement(string request, string meterId,
    DateTimeOffset timestamp);

  string ToPushRequest(IMeasurement measurement);
}
