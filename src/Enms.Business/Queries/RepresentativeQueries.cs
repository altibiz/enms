using System.Linq.Expressions;
using System.Security.Claims;
using Enms.Business.Conversion;
using Enms.Business.Extensions;
using Enms.Business.Models;
using Enms.Business.Models.Composite;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Entities;
using Enms.Data.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Users;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
using YesSql;
using ISession = YesSql.ISession;

namespace Enms.Business.Queries;

public class RepresentativeQueries(
  EnmsDataDbContext context,
  UserManager<IUser> userManager,
  ISession session
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
      .QueryPaged(
        RepresentativeModelEntityConverterExtensions.ToModel,
        filter,
        pageNumber,
        pageCount
      );
  }

  public async Task<PaginatedList<RepresentativeModel>> UserRepresentatives(
    Expression<Func<RepresentativeEntity, bool>>? filter = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
  {
    return await context.Representatives
      .Where(entity => entity.Role == RoleEntity.UserRepresentative)
      .QueryPaged(
        RepresentativeModelEntityConverterExtensions.ToModel,
        filter,
        pageNumber,
        pageCount
      );
  }

  public async Task<UserModel?> UserByClaimsPrincipal(
    ClaimsPrincipal principal)
  {
    return await userManager.GetUserAsync(principal) is { } user
      ? user.ToModel()
      : null;
  }

  public async Task<UserModel?> UserByUserId(string userId)
  {
    return await userManager.FindByIdAsync(userId) is { } user
      ? user.ToModel()
      : null;
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
      Expression<Func<UserIndex, bool>>? filter = null,
      int pageNumber = QueryConstants.StartingPage,
      int pageCount = QueryConstants.DefaultPageCount)
  {
    var users = await session
      .Query<User, UserIndex>()
      .QueryPaged(
        UserModelExtensions.ToModel,
        filter,
        pageNumber,
        pageCount
      );
    var userIds = users.Items
      .Select(user => user.Id)
      .ToList();

    var representatives = await context.Representatives
      .Where(context.PrimaryKeyIn<RepresentativeEntity>(userIds))
      .ToListAsync();

    return users.Items
      .Select(
        user => new MaybeRepresentingUserModel(
          user
        )
        {
          MaybeRepresentative =
            representatives.FirstOrDefault(
                context.PrimaryKeyEqualsCompiled<RepresentativeEntity>(user.Id))
              is { } representative
              ? representative.ToModel()
              : null
        })
      .ToPaginatedList(users.TotalCount);
  }

  public async Task<PaginatedList<RepresentingUserModel>> RepresentingUsers(
    Expression<Func<UserIndex, bool>>? filter = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var users = await session
      .Query<User, UserIndex>()
      .QueryPaged(
        UserModelExtensions.ToModel,
        filter,
        pageNumber,
        pageCount
      );
    var ids = users.Items
      .Select(user => user.Id)
      .ToList();

    var representatives = await context.Representatives
      .Where(context.PrimaryKeyIn<RepresentativeEntity>(ids))
      .ToListAsync();

    return users.Items
      .Select(
        user => new MaybeRepresentingUserModel(
          user
        )
        {
          MaybeRepresentative = representatives
              .FirstOrDefault(
                context
                  .PrimaryKeyEqualsCompiled<RepresentativeEntity>(user.Id)) is
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
    var user = await userManager.GetUserAsync(claimsPrincipal);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(user.GetId()))
        .FirstOrDefaultAsync();
    return representative is null
      ? null
      : new RepresentingUserModel(user.ToModel())
      {
        Representative = representative.ToModel()
      };
  }

  public async Task<RepresentingUserModel?> RepresentingUserByUserId(
    string id)
  {
    var user = await userManager.FindByIdAsync(id);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
        .FirstOrDefaultAsync();
    return representative is null
      ? null
      : new RepresentingUserModel(user.ToModel())
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

    var user = await userManager.FindByIdAsync(representative.Id);
    return user is null
      ? null
      : new RepresentingUserModel(user.ToModel())
      {
        Representative = representative.ToModel()
      };
  }

  public async Task<MaybeRepresentingUserModel?>
    MaybeRepresentingUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
  {
    var user = await userManager.GetUserAsync(claimsPrincipal);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(user.GetId()))
        .FirstOrDefaultAsync();

    return new MaybeRepresentingUserModel(
      user.ToModel()
    )
    {
      MaybeRepresentative = representative?.ToModel()
    };
  }

  public async Task<MaybeRepresentingUserModel?>
    MaybeRepresentingUserByUserId(string id)
  {
    var user = await userManager.FindByIdAsync(id);
    if (user is null)
    {
      return null;
    }

    var representative =
      await context.Representatives
        .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
        .FirstOrDefaultAsync();

    return new MaybeRepresentingUserModel(
      user.ToModel()
    )
    {
      MaybeRepresentative = representative?.ToModel()
    };
  }
}
