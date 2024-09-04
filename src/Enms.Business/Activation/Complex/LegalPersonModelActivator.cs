using Enms.Business.Activation.Base;
using Enms.Business.Models.Complex;

namespace Enms.Business.Activation.Complex;

public class LegalPersonModelActivator : ModelActivator<LegalPersonModel>
{
  public override LegalPersonModel ActivateConcrete()
  {
    return new()
    {
      Name = string.Empty,
      SocialSecurityNumber = string.Empty,
      Address = string.Empty,
      PostalCode = string.Empty,
      City = string.Empty,
      Email = string.Empty,
      PhoneNumber = string.Empty
    };
  }

  public static LegalPersonModel New()
  {
    return new()
    {
      Name = string.Empty,
      SocialSecurityNumber = string.Empty,
      Address = string.Empty,
      PostalCode = string.Empty,
      City = string.Empty,
      Email = string.Empty,
      PhoneNumber = string.Empty
    };
  }
}
