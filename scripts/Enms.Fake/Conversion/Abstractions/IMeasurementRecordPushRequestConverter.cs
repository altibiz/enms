using System.Text.Json.Nodes;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Abstractions;

public interface IMeasurementRecordPushRequestConverter
{
  public bool CanConvertToPushRequest(IMeasurementRecord record);

  public JsonObject ConvertToPushRequest(IMeasurementRecord record);
}
