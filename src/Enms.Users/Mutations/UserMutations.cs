using Enms.Users.Entities;
using Enms.Users.Mutations.Abstractions;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Users;

namespace Enms.Users.Mutations;

// FIXME: updating and deletion

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
  public async Task Update(UserEntity user)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
  {
#pragma warning disable S125 // Sections of code should not be commented out
    // var orchardUser = user.ToUser();
    // orchardUser.SecurityStamp = Guid.NewGuid().ToString();
    // await userManager.UpdateAsync(orchardUser);
#pragma warning restore S125 // Sections of code should not be commented out
  }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
  public async Task Delete(UserEntity user)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
  {
#pragma warning disable S125 // Sections of code should not be commented out
    // var orchardUser = user.ToUser();
    // orchardUser.SecurityStamp = Guid.NewGuid().ToString();
    // await userManager.DeleteAsync(orchardUser);
#pragma warning restore S125 // Sections of code should not be commented out
  }
}
