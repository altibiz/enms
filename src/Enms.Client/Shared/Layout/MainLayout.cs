using Enms.Business.Models.Composite;
using Enms.Business.Queries;
using Enms.Client.Base;
using Enms.Client.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Utilities;

namespace Enms.Client.Shared.Layout;

public partial class MainLayout : EnmsLayoutComponentBase
{
  private readonly MudTheme _theme = new()
  {
    PaletteLight = new PaletteLight
    {
      Primary = new MudColor("#176fc1"),
      Secondary = new MudColor("#338ed2"),
      Tertiary = new MudColor("#a7d6f2"),
      GrayDefault = "#e3e3e3",
      AppbarBackground = "#ffffff",
      AppbarText = "#000000",
    },
    PaletteDark = new PaletteDark
    {
      Primary = new MudColor("#176fc1"),
      Secondary = new MudColor("#338ed2"),
      Tertiary = new MudColor("#a7d6f2"),
      GrayDefault = "#e3e3e3",
      AppbarBackground = "#ffffff",
      AppbarText = "#000000",
    },
    LayoutProperties = new LayoutProperties
    {
      DefaultBorderRadius = "0px"
    },
    Shadows = new Shadow(),
    ZIndex = new ZIndex(),
    Typography = new Typography
    {
      Default = new Default
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = ".875rem",
        FontWeight = 400,
        LineHeight = 1.43,
        LetterSpacing = ".01071em"
      },
      H1 = new H1
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "6rem",
        FontWeight = 300,
        LineHeight = 1.167,
        LetterSpacing = "-.01562em"
      },
      H2 = new H2
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "3.75rem",
        FontWeight = 300,
        LineHeight = 1.2,
        LetterSpacing = "-.00833em"
      },
      H3 = new H3
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "3rem",
        FontWeight = 400,
        LineHeight = 1.167,
        LetterSpacing = "0"
      },
      H4 = new H4
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "2.125rem",
        FontWeight = 400,
        LineHeight = 1.235,
        LetterSpacing = ".00735em"
      },
      H5 = new H5
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "1.5rem",
        FontWeight = 400,
        LineHeight = 1.334,
        LetterSpacing = "0"
      },
      H6 = new H6
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "1.25rem",
        FontWeight = 400,
        LineHeight = 1.6,
        LetterSpacing = ".0075em"
      },
      Button = new Button
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = ".875rem",
        FontWeight = 500,
        LineHeight = 1.75,
        LetterSpacing = ".02857em"
      },
      Body1 = new Body1
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = "1rem",
        FontWeight = 400,
        LineHeight = 1.5,
        LetterSpacing = ".00938em"
      },
      Body2 = new Body2
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = ".875rem",
        FontWeight = 400,
        LineHeight = 1.43,
        LetterSpacing = ".01071em"
      },
      Caption = new Caption
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = ".75rem",
        FontWeight = 400,
        LineHeight = 1.66,
        LetterSpacing = ".03333em"
      },
      Subtitle2 = new Subtitle2
      {
        FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
        FontSize = ".875rem",
        FontWeight = 500,
        LineHeight = 1.57,
        LetterSpacing = ".00714em"
      }
    },
  };

  [CascadingParameter]
  private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  private IServiceScopeFactory ServiceScopeFactory { get; set; } = default!;

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
    return new UserState(
      claimsPrincipal,
      await query.MaybeRepresentingUserByClaimsPrincipal(claimsPrincipal)
    );
  }
}
