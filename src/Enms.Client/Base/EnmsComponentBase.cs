using System.Globalization;
using System.Reflection;
using ApexCharts;
using Enms.Business.Localization.Abstractions;
using Enms.Business.Time;
using Enms.Client.Attributes;
using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public abstract class EnmsComponentBase : ComponentBase
{
  [Inject]
  private ILocalizer Localizer { get; set; } = default!;

  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  protected string Translate(string unlocalized)
  {
    return Localizer.TranslateForCurrentCulture(unlocalized);
  }

  private CultureInfo? _culture;

  protected CultureInfo Culture
  {
    get
    {
      if (_culture is not null)
      {
        return _culture;
      }

      var uri = new Uri(NavigationManager.Uri);
      var culture = uri.Segments[2]?.TrimEnd('/') ?? "en-US";
      var ci = CultureInfo.GetCultureInfo(culture);
      return _culture = ci;
    }
#pragma warning disable S4275 // Getters and setters should access the expected fields
    set
#pragma warning restore S4275 // Getters and setters should access the expected fields
    {
      var uri = new Uri(NavigationManager.Uri);
      var segments = uri.Segments;
      segments[2] = $"{value}/";
      var path = string.Join("", segments);
      NavigationManager.NavigateTo(path);
    }
  }

  protected void Home()
  {
    NavigationManager.NavigateTo($"/app/{Culture}");
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

    var numberFormatInfo = (NumberFormatInfo)Culture.NumberFormat.Clone();
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

    var numberFormatInfo = (NumberFormatInfo)Culture.NumberFormat.Clone();
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

    var cultureInfo = Culture;

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

    var withTimezone = dateTimeOffset
      .Value
      .ToOffset(DateTimeOffsetExtensions.GetOffset(dateTimeOffset.Value));

    return withTimezone.ToString("dd. MM. yyyy. HH:mm", Culture);
  }

  protected static DateTimeOffset DateTimeChart(DateTimeOffset dateTimeOffset)
  {
    return dateTimeOffset.UtcDateTime.Add(
      DateTimeOffsetExtensions.GetOffset(dateTimeOffset));
  }

  protected IEnumerable<NavigationDescriptor> GetNavigationDescriptors()
  {
    foreach (var type in typeof(App).Assembly.GetTypes())
    {
      if (type.GetCustomAttribute(typeof(RouteAttribute)) is RouteAttribute
          routeAttribute
        && type.GetCustomAttribute(typeof(NavigationAttribute)) is
          NavigationAttribute navigationAttribute
        && navigationAttribute.Title is not null)
      {
        yield return new NavigationDescriptor(
          navigationAttribute.Title,
          $"/app/{Culture}" + routeAttribute.Template
        );
      }
    }
  }

  protected record NavigationDescriptor(string Title, string Route);
}
