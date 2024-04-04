using Enms.Business.Iot;
using Enms.Fake.Conversion.Base;
using Enms.Fake.Records;

namespace Enms.Fake.Conversion;

public class EgaugeMeasurementRecordPushRequestConverter
  : MeasurementRecordPushRequestConverter<EgaugeMeasurementRecord,
    EgaugePushRequest>
{
  protected override EgaugePushRequest ConvertToPushRequest(
    EgaugeMeasurementRecord record)
  {
    return new EgaugePushRequest();
  }
}
