using System.Text.Json.Nodes;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Abstractions;

public interface IMeasurementRecordPushRequestConverter
{
  public bool CanConvertToPushRequest(string meterId);

  public JsonNode ConvertToPushRequest(
    string meterId,
    IEnumerable<IMeasurementRecord> records);
}
