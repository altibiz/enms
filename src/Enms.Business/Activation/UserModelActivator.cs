using Enms.Business.Activation.Base;
using Enms.Business.Models;

namespace Enms.Business.Activation;

public class UserModelActivator : ModelActivator<UserModel>
{
  public override UserModel ActivateConcrete()
  {
    return new UserModel
    {
      Id = default!,
      UserName = "",
      Email = "",
      Roles = new List<string>()
    };
  }
}
