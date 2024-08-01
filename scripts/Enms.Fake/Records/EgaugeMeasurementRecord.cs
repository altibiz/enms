using Enms.Business.Math;
using Enms.Fake.Records.Base;

namespace Enms.Fake.Records;

public class EgaugeMeasurementRecord : MeasurementRecord
{
  public override TariffMeasure<decimal> ActiveEnergy_Wh
  {
    get { return TariffMeasure<decimal>.Null; }
  }

  public override TariffMeasure<decimal> ReactiveEnergy_VARh
  {
    get { return TariffMeasure<decimal>.Null; }
  }

  public override TariffMeasure<decimal> ApparentEnergy_VAh
  {
    get { return TariffMeasure<decimal>.Null; }
  }
#pragma warning disable CA1707
  public required decimal VoltageL1AnyT0_V { get; set; }
  public required decimal VoltageL2AnyT0_V { get; set; }
  public required decimal VoltageL3AnyT0_V { get; set; }
  public required decimal CurrentL1AnyT0_A { get; set; }
  public required decimal CurrentL2AnyT0_A { get; set; }
  public required decimal CurrentL3AnyT0_A { get; set; }
  public required decimal ActivePowerL1NetT0_W { get; set; }
  public required decimal ActivePowerL2NetT0_W { get; set; }
  public required decimal ActivePowerL3NetT0_W { get; set; }
  public required decimal ApparentPowerL1NetT0_W { get; set; }
  public required decimal ApparentPowerL2NetT0_W { get; set; }
  public required decimal ApparentPowerL3NetT0_W { get; set; }
#pragma warning restore CA1707
}
