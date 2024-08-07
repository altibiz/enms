using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public abstract class EnmsOwningComponentBase : OwningComponentBase
{
  private readonly EnmsComponentLocalizer _localizer = new();

  public string Translate(string unlocalized)
  {
    return _localizer[unlocalized];
  }
}
