using Enms.Business.Conversion;
using Enms.Business.Models.Composite;
using Enms.Business.Mutations.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities;
using Enms.Data.Extensions;
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
    if (representative is null)
    {
      return;
    }

    context.AddTracked(representative);
    await context.SaveChangesAsync();
  }

  public async Task Update(MaybeRepresentingUserModel model)
  {
    await userMutations.Update(model.User.ToEntity());

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is null)
    {
      return;
    }

    var fromDatabase = context.Representatives
      .FirstOrDefault(context
        .PrimaryKeyEquals<RepresentativeEntity>(representative.Id));
    if (fromDatabase is not null)
    {
      context.UpdateTracked(representative);
      await context.SaveChangesAsync();
    }
    else
    {
      context.AddTracked(representative);
      await context.SaveChangesAsync();
    }
  }

  public async Task Delete(MaybeRepresentingUserModel model)
  {
    await userMutations.Delete(model.User.ToEntity());

    var representative = model.MaybeRepresentative?.ToEntity();
    if (representative is null)
    {
      return;
    }

    var fromDatabase = context.Representatives
      .FirstOrDefault(context
        .PrimaryKeyEquals<RepresentativeEntity>(representative.Id));
    if (fromDatabase is null)
    {
      return;
    }

    context.RemoveTracked(representative);
    await context.SaveChangesAsync();
  }
}
