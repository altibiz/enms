using Enms.Business.Queries;
using Enms.Client.Base;
using Enms.Client.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Enms.Client.Shared.Layout;

public partial class MainLayout : EnmsLayoutComponentBase
{
  [CascadingParameter]
  private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  private IServiceScopeFactory ServiceScopeFactory { get; set; } = default!;

  private MainLayoutState LayoutState { get; set; } = default!;

  private ThemeState ThemeState { get; set; } = default!;

  protected override void OnInitialized()
  {
    LayoutState = new MainLayoutState(
      IsUserDrawerOpen: false,
      IsLocalizationDrawerOpen: false,
      IsNavigationDrawerOpen: false,
      SetUserDrawerOpen: isUserDrawerOpen =>
      {
        LayoutState = LayoutState with
        {
          IsUserDrawerOpen = isUserDrawerOpen
        };
        InvokeAsync(StateHasChanged);
      },
      SetLocalizationDrawerOpen: isLocalizationDrawerOpen =>
      {
        LayoutState = LayoutState with
        {
          IsLocalizationDrawerOpen = isLocalizationDrawerOpen
        };
        InvokeAsync(StateHasChanged);
      },
      SetNavigationDrawerOpen: isNavigationDrawerOpen =>
      {
        LayoutState = LayoutState with
        {
          IsNavigationDrawerOpen = isNavigationDrawerOpen
        };
        InvokeAsync(StateHasChanged);
      }
    );

    ThemeState = new ThemeState(
      ThemeState.Default(),
      (theme) =>
      {
        ThemeState = ThemeState with { Theme = theme };
        InvokeAsync(StateHasChanged);
      }
    );
  }

  private async Task<UserState?> LoadAsync()
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
      NavigationManager.NavigateTo($"/login?returnUrl={NavigationManager.Uri}");
      return default;
    }

    await using var scope = ServiceScopeFactory.CreateAsyncScope();
    var query =
      scope.ServiceProvider.GetRequiredService<RepresentativeQueries>();
    var user = await query.MaybeRepresentingUserByClaimsPrincipal(
      claimsPrincipal);
    if (user is null)
    {
      NavigationManager.NavigateTo($"/login?returnUrl={NavigationManager.Uri}");
      return default;
    }

    return new UserState(claimsPrincipal, user);
  }
}
