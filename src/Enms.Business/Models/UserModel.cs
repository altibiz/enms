using System.ComponentModel.DataAnnotations;
using OrchardCore.Users;
using OrchardCore.Users.Models;

namespace Enms.Business.Models;

public class UserModel
{
  [Required]
  public required string Id { get; set; }

  [Required]
  public required string UserName { get; set; }

  [Required]
  public required string Email { get; set; }

  [Required]
  public required List<string> Roles { get; set; }

  public static UserModel New()
  {
    return new UserModel
    {
      Id = default!,
      UserName = "",
      Email = "",
      Roles = new()
    };
  }
}

public static class UserModelExtensions
{
  public static UserModel ToModel(this IUser abstractUser)
  {
    return abstractUser is User user
      ? new UserModel
      {
        Id = user.UserId,
        UserName = user.UserName,
        Email = user.Email,
        Roles = user.RoleNames.ToList()
      }
      : throw new InvalidOperationException(
        "User is not an Orchard Core user"
      );
  }

  public static User ToUser(this UserModel model)
  {
    return new User()
    {
      UserId = model.Id,
      UserName = model.UserName,
      Email = model.Email,
      RoleNames = model.Roles.ToList()
    };
  }

  public static string GetId(this IUser abstractUser)
  {
    return abstractUser is User user
      ? user.UserId
      : throw new InvalidOperationException(
        "User is not an Orchard Core user"
      );
  }
}
