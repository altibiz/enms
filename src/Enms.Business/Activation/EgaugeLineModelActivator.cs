using Enms.Business.Activation.Base;
using Enms.Business.Models;

namespace Enms.Business.Activation;

public class EgaugeLineModelActivator : ModelActivator<EgaugeLineModel>
{
  public override EgaugeLineModel ActivateConcrete()
  {
    return New();
  }

  public static EgaugeLineModel New()
  {
    return new EgaugeLineModel
    {
      Id = default!,
      Title = "",
      CreatedOn = DateTimeOffset.UtcNow,
      CreatedById = default!,
      LastUpdatedOn = default!,
      LastUpdatedById = default!,
      IsDeleted = false,
      DeletedOn = null,
      DeletedById = null,
      ConnectionPower_W = 0,
      Phases = new HashSet<PhaseModel>(),
      MeasurementValidatorId = default!,
      LineId = default!,
      MeterId = default!,
      OwnerId = default!
    };
  }
}
