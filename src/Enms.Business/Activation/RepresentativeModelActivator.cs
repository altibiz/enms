using Enms.Business.Activation.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;

namespace Enms.Business.Activation;

public class RepresentativeModelActivator : ModelActivator<RepresentativeModel>
{
  public override RepresentativeModel ActivateConcrete()
  {
    return new RepresentativeModel
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
      Role = RoleModel.UserRepresentative,
      Name = "",
      SocialSecurityNumber = "",
      Address = "",
      City = "",
      PostalCode = "",
      Email = "",
      PhoneNumber = ""
    };
  }
}
