using System.Xml.Linq;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IPushRequestMeasurementConverter
{
  bool CanConvert(string meterId);

  IEnumerable<IMeasurement> ToMeasurements(
    string meterId,
    XDocument request,
    DateTimeOffset timestamp);

  XDocument ToPushRequest(IEnumerable<IMeasurement> measurement);
}
