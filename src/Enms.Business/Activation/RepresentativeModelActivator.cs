using Enms.Business.Activation.Base;
using Enms.Business.Activation.Complex;
using Enms.Business.Models;
using Enms.Business.Models.Enums;

namespace Enms.Business.Activation;

public class RepresentativeModelActivator : ModelActivator<RepresentativeModel>
{
  public override RepresentativeModel ActivateConcrete()
  {
    return New();
  }

  public static RepresentativeModel New()
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
      Topics = RoleModel.UserRepresentative.ToTopics(),
      PhysicalPerson = PhysicalPersonModelActivator.New(),
      NetworkUserId = null
    };
  }

  public static RepresentativeModel New(UserModel user)
  {
    var role = RoleModel.UserRepresentative;

    return new RepresentativeModel
    {
      Id = user.Id,
      Title = user.UserName,
      CreatedOn = DateTimeOffset.UtcNow,
      CreatedById = user.Id,
      LastUpdatedOn = default!,
      LastUpdatedById = null,
      IsDeleted = false,
      DeletedOn = null,
      DeletedById = null,
      Role = role,
      Topics = role.ToTopics(),
      PhysicalPerson = PhysicalPersonModelActivator.New(user),
      NetworkUserId = null
    };
  }
}
