using Enms.Business.Conversion;
using Enms.Business.Models;
using Enms.Business.Models.Composite;
using Enms.Business.Mutations.Abstractions;
using Enms.Data;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Users;

namespace Enms.Business.Mutations;

public class RepresentativeMutations(
  EnmsDataDbContext context,
  UserManager<IUser> userManager
) : IMutations
{
  public async Task Create(MaybeRepresentingUserModel model)
  {
    var user = model.User.ToUser();
    await userManager.CreateAsync(user);

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is not null)
    {
      context.Representatives.Add(representative);
      await context.SaveChangesAsync();
    }
  }

  public async Task Update(MaybeRepresentingUserModel model)
  {
    var user = model.User.ToUser();
    await userManager.UpdateAsync(user);

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is not null)
    {
      context.Representatives.Update(representative);
      await context.SaveChangesAsync();
    }
  }

  public async Task Delete(MaybeRepresentingUserModel model)
  {
    var user = model.User.ToUser();
    await userManager.DeleteAsync(user);

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is not null)
    {
      context.Representatives.Remove(representative);
      await context.SaveChangesAsync();
    }
  }
}
