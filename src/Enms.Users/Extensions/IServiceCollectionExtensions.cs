using Enms.Users.Mutations;
using Enms.Users.Mutations.Abstractions;
using Enms.Users.Queries;
using Enms.Users.Queries.Abstractions;

namespace Enms.Users.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsUsers(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.AddScoped<IUserQueries, UserQueries>();
    services.AddScoped<IUserMutations, UserMutations>();

    return services;
  }
}
