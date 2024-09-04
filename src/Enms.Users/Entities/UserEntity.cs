using Enms.Users.Entities.Abstractions;
using OrchardCore.Users;
using OrchardCore.Users.Models;

namespace Enms.Users.Entities;

public record UserEntity(
  string Id,
  string UserName,
  string Email
) : IEntity;

public static class UserModelExtensions
{
  public static UserEntity ToEntity(this IUser abstractUser)
  {
    return abstractUser is User user
      ? new UserEntity(
        user.UserId,
        user.UserName,
        user.Email
      )
      : throw new InvalidOperationException(
        "User is not an Orchard Core user"
      );
  }

  public static User ToUser(this UserEntity entity)
  {
    return new User
    {
      UserId = entity.Id,
      UserName = entity.UserName,
      Email = entity.Email
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
