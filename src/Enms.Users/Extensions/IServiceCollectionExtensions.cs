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
    // Mutations
    services.AddScoped<IUserMutations, UserMutations>();

    // Queries
    services.AddScoped<IUserQueries, UserQueries>();

    return services;
  }
}
