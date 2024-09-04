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
    await userManager.CreateAsync(user.ToUser());
  }

  public async Task Update(UserEntity user)
  {
    await userManager.UpdateAsync(user.ToUser());
  }

  public async Task Delete(UserEntity user)
  {
    await userManager.DeleteAsync(user.ToUser());
  }
}
