using System.Globalization;
using Enms.Business.Time;
using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public abstract class EnmsComponentBase : ComponentBase
{
  private readonly EnmsComponentLocalizer _localizer = new();

  public string Translate(string unlocalized)
  {
    return _localizer[unlocalized];
  }

  protected static string DecimalString(decimal? number, int places = 2)
  {
    if (number is null)
    {
      return "";
    }

    var cultureInfo = new CultureInfo("hr-HR");

    var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
    numberFormatInfo.NumberGroupSeparator = ".";
    numberFormatInfo.NumberDecimalDigits = places;

    var roundedNumber = Math.Round(number.Value, places);
    return roundedNumber.ToString("N", numberFormatInfo);
  }

  protected static string DateString(DateTimeOffset? dateTimeOffset)
  {
    if (dateTimeOffset is null)
    {
      return "";
    }

    var cultureInfo = new CultureInfo("hr-HR");

    var withTimezone = dateTimeOffset
      .Value
      .ToOffset(DateTimeOffsetExtensions.DefaultOffset);

    return withTimezone.ToString("dd. MM. yyyy.", cultureInfo);
  }

  protected static DateTimeOffset DateTimeGraph(DateTimeOffset dateTimeOffset)
  {
    return dateTimeOffset.UtcDateTime.Add(
      DateTimeOffsetExtensions.DefaultOffset);
  }
}
