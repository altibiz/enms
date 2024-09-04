using System.Security.Claims;
using Enms.Users.Entities;
using Enms.Users.Queries.Abstractions;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Users;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
using YesSql;
using ISession = YesSql.ISession;

namespace Enms.Users.Queries;

public class UserQueries(
  UserManager<IUser> userManager,
  ISession session
) : IUserQueries
{
  public async Task<UserEntity?> UserByClaimsPrincipal(
    ClaimsPrincipal principal)
  {
    return await userManager.GetUserAsync(principal) is { } user
      ? user.ToEntity()
      : null;
  }

  public async Task<UserEntity?> UserByUserId(string userId)
  {
    return await userManager.FindByIdAsync(userId) is { } user
      ? user.ToEntity()
      : null;
  }

  public async Task<(List<UserEntity> Items, int TotalCount)> Users(
    int pageNumber,
    int pageSize)
  {
    var ordered = session
      .Query<User, UserIndex>()
      .OrderBy(index => index.DocumentId);

    var count = await ordered.CountAsync();
    var filtered = ordered
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize);

    var users = await filtered.ListAsync();

    return (users.Select(user => user.ToEntity()).ToList(), count);
  }
}
