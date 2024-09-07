using Enms.Business.Activation.Base;
using Enms.Business.Models.Complex;
using Enms.Business.Models.Enums;

namespace Enms.Business.Activation.Complex;

public class PeriodModelActivator : ModelActivator<PeriodModel>
{
  public override PeriodModel ActivateConcrete()
  {
    return new PeriodModel
    {
      Duration = DurationModel.Second,
      Multiplier = 1
    };
  }

  public static PeriodModel New()
  {
    return new PeriodModel
    {
      Duration = DurationModel.Second,
      Multiplier = 1
    };
  }
}
