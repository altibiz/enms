using System.Globalization;
using ApexCharts;
using Enms.Business.Localization.Abstractions;
using Enms.Business.Time;
using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public abstract class EnmsComponentBase : ComponentBase
{
  [Inject]
  private ILocalizer Localizer { get; set; } = default!;

  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  public string Translate(string unlocalized)
  {
    return Localizer.TranslateForCurrentCulture(unlocalized);
  }

  private CultureInfo Culture()
  {
    var uri = new Uri(NavigationManager.Uri);
    var culture = uri.Segments[2]?.TrimEnd('/') ?? "en-US";
    var ci = CultureInfo.GetCultureInfo(culture);
    return ci;
  }

  protected static ApexChartOptions<T> NewApexChartOptions<T>()
    where T : class
  {
    var options = new ApexChartOptions<T>
    {
      Blazor = new ApexChartsBlazorOptions
      {
        JavascriptPath = "/_content/Blazor-ApexCharts/js/blazor-apexcharts.js"
      }
    };

    return options;
  }

  protected string NumericString(decimal? number, int places = 2)
  {
    if (number is null)
    {
      return "";
    }

    var cultureInfo = Culture();

    var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
    numberFormatInfo.NumberGroupSeparator = ".";
    numberFormatInfo.NumberDecimalDigits = places;

    var roundedNumber = Math.Round(number.Value, places);
    return roundedNumber.ToString("N", numberFormatInfo);
  }

  protected string NumericString(float? number, int places = 2)
  {
    if (number is null)
    {
      return "";
    }

    var cultureInfo = Culture();

    var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
    numberFormatInfo.NumberGroupSeparator = ".";
    numberFormatInfo.NumberDecimalDigits = places;

    var roundedNumber = Math.Round(number.Value, places);
    return roundedNumber.ToString("N", numberFormatInfo);
  }

  protected string DateString(DateTimeOffset? dateTimeOffset)
  {
    if (dateTimeOffset is null)
    {
      return "";
    }

    var cultureInfo = Culture();

    var withTimezone = dateTimeOffset
      .Value
      .ToOffset(DateTimeOffsetExtensions.GetOffset(dateTimeOffset.Value));

    return withTimezone.ToString("dd. MM. yyyy.", cultureInfo);
  }

  protected string DateTimeString(DateTimeOffset? dateTimeOffset)
  {
    if (dateTimeOffset is null)
    {
      return "";
    }

    var cultureInfo = Culture();

    var withTimezone = dateTimeOffset
      .Value
      .ToOffset(DateTimeOffsetExtensions.GetOffset(dateTimeOffset.Value));

    return withTimezone.ToString("dd. MM. yyyy. HH:mm", cultureInfo);
  }

  protected static DateTimeOffset DateTimeGraph(DateTimeOffset dateTimeOffset)
  {
    return dateTimeOffset.UtcDateTime.Add(
      DateTimeOffsetExtensions.GetOffset(dateTimeOffset));
  }
}
