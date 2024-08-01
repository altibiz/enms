using System.Text.Json.Nodes;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IPushRequestMeasurementConverter
{
  bool CanConvert(string meterId);

  IEnumerable<IMeasurement> ToMeasurements(
    string meterId,
    JsonNode request,
    DateTimeOffset timestamp);

  JsonNode ToPushRequest(IEnumerable<IMeasurement> measurement);
}
