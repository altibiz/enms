using System.Globalization;

namespace Enms.Business.Localization.Abstractions;

public interface IEnmsLocalizer
{
  public string this[string notLocalized] { get; }

  public string ForCulture(CultureInfo culture, string notLocalized);
}
