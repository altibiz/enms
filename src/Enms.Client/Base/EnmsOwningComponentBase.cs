using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public abstract class EnmsOwningComponentBase : OwningComponentBase
{
  public static EnmsComponentLocalizer T
  {
    get { return new EnmsComponentLocalizer(); }
  }
}
