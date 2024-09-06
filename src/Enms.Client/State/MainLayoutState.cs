namespace Enms.Client.State;

public record MainLayoutState(
  bool IsUserDrawerOpen,
  bool IsLocalizationDrawerOpen,
  bool IsNavigationDrawerOpen,
  Action<bool> SetUserDrawerOpen,
  Action<bool> SetLocalizationDrawerOpen,
  Action<bool> SetNavigationDrawerOpen
);
