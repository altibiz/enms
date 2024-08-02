using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Fake.Conversion.Base;
using Enms.Fake.Records;

namespace Enms.Fake.Conversion;

public class EgaugeMeasurementRecordMeasurementConverter
  : MeasurementRecordMeasurementConverter<EgaugeMeasurementRecord>
{
  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  protected override IMeasurement ConvertToMeasurementConcrete(
    EgaugeMeasurementRecord record)
  {
    return new EgaugeMeasurementModel
    {
      MeterId = record.MeterId,
      LineId = record.LineId,
      Timestamp = record.Timestamp,
      VoltageL1AnyT0_V = record.VoltageL1AnyT0_V,
      VoltageL2AnyT0_V = record.VoltageL2AnyT0_V,
      VoltageL3AnyT0_V = record.VoltageL3AnyT0_V,
      CurrentL1AnyT0_A = record.CurrentL1AnyT0_A,
      CurrentL2AnyT0_A = record.CurrentL2AnyT0_A,
      CurrentL3AnyT0_A = record.CurrentL3AnyT0_A,
      ActivePowerL1NetT0_W = record.ActivePowerL1NetT0_W,
      ActivePowerL2NetT0_W = record.ActivePowerL2NetT0_W,
      ActivePowerL3NetT0_W = record.ActivePowerL3NetT0_W,
      ApparentPowerL1NetT0_W = record.ApparentPowerL1NetT0_W,
      ApparentPowerL2NetT0_W = record.ApparentPowerL2NetT0_W,
      ApparentPowerL3NetT0_W = record.ApparentPowerL3NetT0_W
    };
  }
}
