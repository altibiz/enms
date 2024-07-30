using Enms.Business.Iot;
using Enms.Fake.Conversion.Base;
using Enms.Fake.Records;

namespace Enms.Fake.Conversion;

public class EgaugeMeasurementRecordPushRequestConverter
  : MeasurementRecordPushRequestConverter<EgaugeMeasurementRecord,
    object>
{
  protected override object ConvertToPushRequest(
    EgaugeMeasurementRecord record)
  {
    return new object();
  }
}
