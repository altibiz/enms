using Enms.Client.State;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;

namespace Enms.Client.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsClient(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    // Blazor
    services.AddBlazor(builder);

    return services;
  }

  private static void AddBlazor(
    this IServiceCollection services,
    IHostApplicationBuilder builder
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
          if (builder.Environment.IsDevelopment())
          {
            options.DetailedErrors = true;
          }
        })
      .AddHubOptions(
        options =>
        {
          if (builder.Environment.IsDevelopment())
          {
            options.EnableDetailedErrors = true;
          }
        });

    services.AddMudServices();

    services.AddCascadingAuthenticationState();
    services.AddCascadingValue(_ => default(UserState));
    services.AddCascadingValue(_ => default(RepresentativeState));
    services.AddCascadingValue(_ => default(MainLayoutState));
    services.AddCascadingValue(_ => default(ThemeState));
  }
}
