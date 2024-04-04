using System.Text.Json;
using System.Text.Json.Nodes;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Base;

public abstract class MeasurementRecordPushRequestConverter<TRecord,
  TPushRequest> : IMeasurementRecordPushRequestConverter
  where TRecord : IMeasurementRecord
{
  public bool CanConvertToPushRequest(IMeasurementRecord record)
  {
    return record is TRecord;
  }

  public JsonObject ConvertToPushRequest(IMeasurementRecord record)
  {
    return JsonSerializer
          .SerializeToNode(ConvertToPushRequest((TRecord)record)!) as
        JsonObject
      ?? throw new InvalidOperationException();
  }

  protected abstract TPushRequest ConvertToPushRequest(TRecord record);
}
