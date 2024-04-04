using System.Xml.Linq;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IPushRequestMeasurementConverter
{
  bool CanConvert(string meterId);

  IMeasurement ToMeasurement(
    XDocument request,
    string meterId,
    DateTimeOffset timestamp);

  XDocument ToPushRequest(IMeasurement measurement);
}
