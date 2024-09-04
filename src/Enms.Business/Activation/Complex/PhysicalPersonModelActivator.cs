using Enms.Business.Activation.Base;
using Enms.Business.Models;
using Enms.Business.Models.Complex;

namespace Enms.Business.Activation.Complex;

public class PhysicalPersonModelActivator : ModelActivator<PhysicalPersonModel>
{
  public override PhysicalPersonModel ActivateConcrete()
  {
    return New();
  }

  public static PhysicalPersonModel New()
  {
    return new PhysicalPersonModel
    {
      Email = "",
      PhoneNumber = "",
      Name = ""
    };
  }

  public static PhysicalPersonModel New(UserModel user)
  {
    return new PhysicalPersonModel
    {
      Name = "",
      Email = user.Email,
      PhoneNumber = ""
    };
  }
}
