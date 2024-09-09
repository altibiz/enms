using System.Globalization;

namespace Enms.Client.ViewModels;

public class AppViewModel
{
  public CultureInfo Culture { get; set; } = default!;

  public string LogoutToken { get; set; } = default!;
}
