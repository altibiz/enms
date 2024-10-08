using Enms.Business.Models;
using Enms.Fake.Correction.Base;

namespace Enms.Fake.Correction;

public class EgaugeMeasurementModelCorrector
  : MeasurementCorrector<EgaugeMeasurementModel>
{
  protected override EgaugeMeasurementModel CorrectCumulatives(
    DateTimeOffset timestamp,
    EgaugeMeasurementModel measurementRecord,
    EgaugeMeasurementModel firstMeasurementRecord,
    EgaugeMeasurementModel lastMeasurementRecord
  )
  {
    return measurementRecord;
  }

  protected override EgaugeMeasurementModel CopyCorrectId(
    EgaugeMeasurementModel record,
    string meterId,
    string lineId)
  {
    return new EgaugeMeasurementModel
    {
      MeterId = meterId,
      LineId = lineId,
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
