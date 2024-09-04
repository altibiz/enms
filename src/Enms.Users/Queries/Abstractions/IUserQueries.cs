using System.Security.Claims;
using Enms.Users.Entities;

namespace Enms.Users.Queries.Abstractions;

public interface IUserQueries
{
  Task<UserEntity?> UserByClaimsPrincipal(ClaimsPrincipal principal);
  Task<UserEntity?> UserByUserId(string userId);

  Task<(List<UserEntity> Items, int TotalCount)> Users(
    int pageNumber,
    int pageSize);
}
