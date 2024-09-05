using System.Linq.Expressions;
using System.Security.Claims;
using Enms.Business.Conversion;
using Enms.Business.Extensions;
using Enms.Business.Models;
using Enms.Business.Models.Composite;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities;
using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Enms.Users.Queries;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries;

public class RepresentativeQueries(
  DataDbContext context,
  UserQueries userQueries
) : IQueries
{
  public async Task<RepresentativeModel?> RepresentativeById(string id)
  {
    return await context.Representatives
      .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
      .FirstOrDefaultAsync() is { } entity
      ? entity.ToModel()
      : null;
  }

  public async Task<PaginatedList<RepresentativeModel>> OperatorRepresentatives(
    Expression<Func<RepresentativeEntity, bool>>? filter = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
  {
    return await context.Representatives
      .Where(entity => entity.Role == RoleEntity.OperatorRepresentative)
      .OrderBy(context.PrimaryKeyOf<RepresentativeEntity>())
      .QueryPaged(
        RepresentativeModelEntityConverterExtensions.ToModel,
        filter,
        pageNumber,
        pageCount
      );
  }

  public async Task<RepresentativeModel?> RepresentativeByUserId(
    string userId)
  {
    return await context.Representatives
      .Where(context.PrimaryKeyEquals<RepresentativeEntity>(userId))
      .FirstOrDefaultAsync() is { } entity
      ? entity.ToModel()
      : null;
  }

  public async Task<PaginatedList<MaybeRepresentingUserModel>>
    MaybeRepresentingUsers(
      int pageNumber = QueryConstants.StartingPage,
      int pageCount = QueryConstants.DefaultPageCount)
  {
    var users = await userQueries.Users(pageNumber, pageCount);
    var userIds = users.Items
      .Select(user => user.Id)
      .ToList();

    var representatives = await context.Representatives
      .Where(context.PrimaryKeyIn<RepresentativeEntity>(userIds))
      .ToListAsync();

    return users.Items
      .Select(
        user => new MaybeRepresentingUserModel(user.ToModel())
        {
          MaybeRepresentative = representatives
              .FirstOrDefault(
                context.PrimaryKeyEqualsCompiled<RepresentativeEntity>(user.Id))
            is { } representative
            ? representative.ToModel()
            : null
        })
      .ToPaginatedList(users.TotalCount);
  }

  public async Task<PaginatedList<RepresentingUserModel>> RepresentingUsers(
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var users = await userQueries.Users(pageNumber, pageCount);
    var ids = users.Items
      .Select(user => user.Id)
      .ToList();

    var representatives = await context.Representatives
      .Where(context.PrimaryKeyIn<RepresentativeEntity>(ids))
      .ToListAsync();

    return users.Items
      .Select(
        user => new MaybeRepresentingUserModel(
          user.ToModel()
        )
        {
          MaybeRepresentative = representatives
              .FirstOrDefault(
                context.PrimaryKeyInCompiled<RepresentativeEntity>(ids)) is
            { } representative
            ? representative.ToModel()
            : null
        })
      .Where(
        maybeRepresentingUser =>
          maybeRepresentingUser.MaybeRepresentative is not null)
      .Select(
        maybeRepresentingUser => new RepresentingUserModel(
          maybeRepresentingUser.User
        )
        {
          Representative = maybeRepresentingUser.MaybeRepresentative!
        })
      .ToPaginatedList(users.TotalCount);
  }

  public async Task<RepresentingUserModel?>
    RepresentingUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
  {
    var user = await userQueries.UserByClaimsPrincipal(claimsPrincipal);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(user.Id))
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return null;
    }

    return new RepresentingUserModel(
      user.ToModel()
    )
    {
      Representative = representative.ToModel()
    };
  }

  public async Task<RepresentingUserModel?> RepresentingUserByUserId(
    string id)
  {
    var user = await userQueries.UserByUserId(id);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return null;
    }

    return new RepresentingUserModel(
      user.ToModel()
    )
    {
      Representative = representative.ToModel()
    };
  }

  public async Task<RepresentingUserModel?>
    RepresentingUserByRepresentativeId(string id)
  {
    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return null;
    }

    var user = await userQueries.UserByUserId(representative.Id);
    if (user is null)
    {
      return null;
    }

    return new RepresentingUserModel(
      user.ToModel()
    )
    {
      Representative = representative.ToModel()
    };
  }

  public async Task<MaybeRepresentingUserModel?>
    MaybeRepresentingUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
  {
    var user = await userQueries.UserByClaimsPrincipal(claimsPrincipal);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(user.Id))
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return new MaybeRepresentingUserModel(user.ToModel());
    }

    return new RepresentingUserModel(
      user.ToModel()
    )
    {
      Representative = representative.ToModel()
    };
  }

  public async Task<MaybeRepresentingUserModel?>
    MaybeRepresentingUserByUserId(string id)
  {
    var user = await userQueries.UserByUserId(id);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return new MaybeRepresentingUserModel(user.ToModel());
    }

    return new RepresentingUserModel(
      user.ToModel()
    )
    {
      Representative = representative.ToModel()
    };
  }
}
