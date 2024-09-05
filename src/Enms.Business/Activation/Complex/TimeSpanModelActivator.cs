using Enms.Business.Activation.Base;
using Enms.Business.Models.Complex;
using Enms.Business.Models.Enums;

namespace Enms.Business.Activation.Complex;

public class TimeSpanModelActivator : ModelActivator<TimeSpanModel>
{
  public override TimeSpanModel ActivateConcrete()
  {
    return new TimeSpanModel
    {
      Duration = DurationModel.Second,
      Multiplier = 1
    };
  }

  public static TimeSpanModel New()
  {
    return new TimeSpanModel
    {
      Duration = DurationModel.Second,
      Multiplier = 1
    };
  }
}
