using System.Xml.Linq;
using Enms.Fake.Conversion.Base;
using Enms.Fake.Records;

namespace Enms.Fake.Conversion;

public class EgaugeMeasurementRecordPushRequestConverter
  : MeasurementRecordPushRequestConverter<EgaugeMeasurementRecord>
{
  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  protected override XDocument ConvertToPushRequestConcrete(
    string meterId,
    IEnumerable<EgaugeMeasurementRecord> record)
  {
    throw new NotImplementedException();
  }
}
