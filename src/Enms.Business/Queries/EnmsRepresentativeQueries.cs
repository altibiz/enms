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
using Enms.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Users;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
using YesSql;
using ISession = YesSql.ISession;

namespace Enms.Business.Queries;

public class EnmsRepresentativeQueries : IEnmsQueries
{
  private readonly EnmsDataDbContext _context;
  private readonly ISession _session;

  private readonly UserManager<IUser> _userManager;

  public EnmsRepresentativeQueries(
    EnmsDataDbContext context,
    UserManager<IUser> userManager,
    ISession session
  )
  {
    _context = context;
    _session = session;
    _userManager = userManager;
  }

  public async Task<RepresentativeModel?> RepresentativeById(string id)
  {
    return await _context.Representatives
      .WithId(id)
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
    return await _context.Representatives
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
    return await _context.Representatives
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
    return await _userManager.GetUserAsync(principal) is { } user
      ? user.ToModel()
      : null;
  }

  public async Task<UserModel?> UserByUserId(string userId)
  {
    return await _userManager.FindByIdAsync(userId) is { } user
      ? user.ToModel()
      : null;
  }

  public async Task<RepresentativeModel?> RepresentativeByUserId(
    string userId)
  {
    return await _context.Representatives
      .WithId(userId)
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
    var users = await _session
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

    var representatives = await _context.Representatives
      .WithIdFrom(userIds)
      .ToListAsync();

    return users.Items
      .Select(
        user => new MaybeRepresentingUserModel(
          user,
          representatives.WithId(user.Id).FirstOrDefault() is { } representative
            ? representative.ToModel()
            : null
        ))
      .ToPaginatedList(users.TotalCount);
  }

  public async Task<PaginatedList<RepresentingUserModel>> RepresentingUsers(
    Expression<Func<UserIndex, bool>>? filter = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var users = await _session
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

    var representatives = await _context.Representatives
      .WithIdFrom(ids)
      .ToListAsync();

    return users.Items
      .Select(
        user => new MaybeRepresentingUserModel(
          user,
          representatives.WithId(user.Id).FirstOrDefault() is { } representative
            ? representative.ToModel()
            : null
        ))
      .Where(
        maybeRepresentingUser =>
          maybeRepresentingUser.Representative is not null)
      .Select(
        maybeRepresentingUser => new RepresentingUserModel(
          maybeRepresentingUser.User,
          maybeRepresentingUser.Representative!
        ))
      .ToPaginatedList(users.TotalCount);
  }

  public async Task<RepresentingUserModel?>
    RepresentingUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
  {
    var user = await _userManager.GetUserAsync(claimsPrincipal);
    if (user is null)
    {
      return null;
    }

    var representative =
      await _context.Representatives
        .WithId(user.GetId())
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return null;
    }

    return new RepresentingUserModel(
      user.ToModel(),
      representative.ToModel()
    );
  }

  public async Task<RepresentingUserModel?> RepresentingUserByUserId(
    string id)
  {
    var user = await _userManager.FindByIdAsync(id);
    if (user is null)
    {
      return null;
    }

    var representative =
      await _context.Representatives
        .WithId(id)
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return null;
    }

    return new RepresentingUserModel(
      user.ToModel(),
      representative.ToModel()
    );
  }

  public async Task<RepresentingUserModel?>
    RepresentingUserByRepresentativeId(string id)
  {
    var representative =
      await _context.Representatives
        .WithId(id)
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return null;
    }

    var user = await _userManager.FindByIdAsync(representative.Id);
    if (user is null)
    {
      return null;
    }

    return new RepresentingUserModel(
      user.ToModel(),
      representative.ToModel()
    );
  }

  public async Task<MaybeRepresentingUserModel?>
    MaybeRepresentingUserByClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
  {
    var user = await _userManager.GetUserAsync(claimsPrincipal);
    if (user is null)
    {
      return null;
    }

    var representative =
      await _context.Representatives
        .WithId(user.GetId())
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return new MaybeRepresentingUserModel(user.ToModel(), null);
    }

    return new MaybeRepresentingUserModel(
      user.ToModel(),
      representative.ToModel()
    );
  }

  public async Task<MaybeRepresentingUserModel?>
    MaybeRepresentingUserByUserId(string id)
  {
    var user = await _userManager.FindByIdAsync(id);
    if (user is null)
    {
      return null;
    }

    var representative =
      await _context.Representatives
        .WithId(id)
        .FirstOrDefaultAsync();
    if (representative is null)
    {
      return new MaybeRepresentingUserModel(user.ToModel(), null);
    }

    return new MaybeRepresentingUserModel(
      user.ToModel(),
      representative.ToModel()
    );
  }
}
