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
#pragma warning restore CA1707
}
