using System.Globalization;

namespace Enms.Business.Localization.Abstractions;

public interface ILocalizer
{
  public string TranslateForInvariantCulture(string notLocalized);

  public string TranslateForCurrentCulture(string notLocalized);

  public string TranslateForCulture(CultureInfo culture, string notLocalized);
}
