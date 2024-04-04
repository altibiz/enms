using Enms.Client.State;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Enms.Client.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsClient(
    this IServiceCollection services,
    bool isDevelopment
  )
  {
    services
      .AddRazorComponents()
      .AddInteractiveServerComponents();

    services
      .AddServerSideBlazor()
      .AddCircuitOptions(
        options =>
        {
          if (isDevelopment)
          {
            options.DetailedErrors = true;
          }
        })
      .AddHubOptions(
        options =>
        {
          if (isDevelopment)
          {
            options.EnableDetailedErrors = true;
          }
        });

    services.AddMudServices();

    services.AddCascadingAuthenticationState();
    services.AddCascadingValue(_ => default(UserState));
    services.AddCascadingValue(_ => default(RepresentativeState));

    return services;
  }
}
