using Enms.Business.Activation.Base;
using Enms.Business.Activation.Complex;
using Enms.Business.Models;

namespace Enms.Business.Activation;

public class NetworkUserModelActivator : ModelActivator<NetworkUserModel>
{
  public override NetworkUserModel ActivateConcrete()
  {
    return New();
  }

  public static NetworkUserModel New()
  {
    return new NetworkUserModel
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
      LegalPerson = LegalPersonModelActivator.New(),
    };
  }
}
