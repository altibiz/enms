using System.Text.Json.Nodes;
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

  protected override JsonNode ConvertToPushRequestConcrete(
    string meterId,
    IEnumerable<EgaugeMeasurementRecord> record)
  {
    throw new NotImplementedException();
  }
}
