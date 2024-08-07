using System.Reflection;
using System.Security.Claims;
using Enms.Business.Models.Composite;
using Enms.Business.Queries;
using Enms.Client.Attributes;
using Enms.Client.Base;
using Enms.Client.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

// TODO: remove hardcoding of /app here

namespace Enms.Client.Shared.Layout;

public partial class MainLayout : EnmsLayoutComponentBase
{
  [CascadingParameter]
  private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  private IServiceProvider Services { get; set; } = default!;

  public static IEnumerable<NavigationDescriptor> GetNavigationDescriptors()
  {
    foreach (var type in typeof(App).Assembly.GetTypes())
    {
      if (type.GetCustomAttribute(typeof(RouteAttribute)) is RouteAttribute
          routeAttribute
        && type.GetCustomAttribute(typeof(NavigationAttribute)) is
          NavigationAttribute navigationAttribute
        && navigationAttribute.Title is not null)
      {
        yield return new NavigationDescriptor(
          navigationAttribute.Title,
          "/app" + routeAttribute.Template
        );
      }
    }
  }

  private async Task<MaybeRepresentingUserModel?> ReadMaybeRepresentingUser()
  {
    if (AuthenticationStateTask is null)
    {
      return default;
    }

    var authenticationState = await AuthenticationStateTask;
    var claimsPrincipal = authenticationState?.User ??
      throw new InvalidOperationException(
        "No claims principal found.");
    if (!(claimsPrincipal.Identity?.IsAuthenticated ?? false))
    {
      NavigationManager.NavigateTo("/login?returnUrl=/app");
      return default;
    }

    await using var scope = Services.CreateAsyncScope();
    var query =
      scope.ServiceProvider.GetRequiredService<RepresentativeQueries>();
    return await query.MaybeRepresentingUserByClaimsPrincipal(claimsPrincipal);
  }

  public record NavigationDescriptor(string Title, string Route);
}
// TODO: remove hardcoding of /app here
