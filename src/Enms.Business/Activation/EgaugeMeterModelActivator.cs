using Enms.Business.Activation.Base;
using Enms.Business.Models;

namespace Enms.Business.Activation;

public class EgaugeMeterModelActivator : ModelActivator<EgaugeMeterModel>
{
  public override EgaugeMeterModel ActivateConcrete()
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
      DeletedById = null
    };
  }
}
