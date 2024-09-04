using Enms.Business.Activation.Base;
using Enms.Business.Activation.Complex;
using Enms.Business.Models;

namespace Enms.Business.Activation;

public class EgaugeMeterModelActivator : ModelActivator<EgaugeMeterModel>
{
  public override EgaugeMeterModel ActivateConcrete()
  {
    return New();
  }

  public static EgaugeMeterModel New()
  {
    return new EgaugeMeterModel
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
      InactivityDuration = TimeSpanModelActivator.New()
    };
  }
}
