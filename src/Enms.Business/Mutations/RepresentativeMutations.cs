using Enms.Business.Conversion;
using Enms.Business.Models.Composite;
using Enms.Business.Mutations.Abstractions;
using Enms.Data.Context;
using Enms.Users.Mutations.Abstractions;

namespace Enms.Business.Mutations;

public class RepresentativeMutations(
  DataDbContext context,
  IUserMutations userMutations
) : IMutations
{
  public async Task Create(MaybeRepresentingUserModel model)
  {
    await userMutations.Create(model.User.ToEntity());

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is not null)
    {
      context.Representatives.Add(representative);
      await context.SaveChangesAsync();
    }
  }

  public async Task Update(MaybeRepresentingUserModel model)
  {
    await userMutations.Update(model.User.ToEntity());

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is not null)
    {
      context.Representatives.Update(representative);
      await context.SaveChangesAsync();
    }
  }

  public async Task Delete(MaybeRepresentingUserModel model)
  {
    await userMutations.Delete(model.User.ToEntity());

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is not null)
    {
      context.Representatives.Remove(representative);
      await context.SaveChangesAsync();
    }
  }
}
