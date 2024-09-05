using Enms.Users.Entities;
using Enms.Users.Mutations.Abstractions;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Users;

namespace Enms.Users.Mutations;

public class UserMutations(
  UserManager<IUser> userManager
) : IUserMutations
{
  public async Task Create(UserEntity user)
  {
    var orchardUser = user.ToUser();
    orchardUser.SecurityStamp = Guid.NewGuid().ToString();
    await userManager.CreateAsync(orchardUser);
  }

  public async Task Update(UserEntity user)
  {
    var orchardUser = user.ToUser();
    orchardUser.SecurityStamp = Guid.NewGuid().ToString();
    await userManager.UpdateAsync(orchardUser);
  }

  public async Task Delete(UserEntity user)
  {
    var orchardUser = user.ToUser();
    orchardUser.SecurityStamp = Guid.NewGuid().ToString();
    await userManager.DeleteAsync(orchardUser);
  }
}
