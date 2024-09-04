using Enms.Business.Time;

namespace Enms.Business.Models.Enums;

public enum ResolutionModel
{
  Minute,
  Hour,
  Day,
  Week,
  Month,
  Year
}

public static class ChartResolutionExtensions
{
  public static int MaxMultiplier(this ResolutionModel resolution)
  {
    return resolution switch
    {
      ResolutionModel.Minute => 60,
      ResolutionModel.Hour => 24,
      ResolutionModel.Day => 30,
      ResolutionModel.Week => 4,
      ResolutionModel.Month => 12,
      ResolutionModel.Year => 2,
      _ => throw new ArgumentOutOfRangeException(
        nameof(resolution),
        resolution,
        null)
    };
  }

  public static string ToTitle(this ResolutionModel resolution, int multiplier)
  {
    if (multiplier == 1)
    {
      return resolution switch
      {
        ResolutionModel.Minute => "Minute",
        ResolutionModel.Hour => "Hour",
        ResolutionModel.Day => "Day",
        ResolutionModel.Week => "Week",
        ResolutionModel.Month => "Month",
        ResolutionModel.Year => "Year",
        _ => throw new ArgumentOutOfRangeException(
          nameof(resolution),
          resolution,
          null)
      };
    }

    return resolution switch
    {
      ResolutionModel.Minute => "Minutes",
      ResolutionModel.Hour => "Hours",
      ResolutionModel.Day => "Days",
      ResolutionModel.Week => "Weeks",
      ResolutionModel.Month => "Months",
      ResolutionModel.Year => "Years",
      _ => throw new ArgumentOutOfRangeException(
        nameof(resolution),
        resolution,
        null)
    };
  }

  public static TimeSpan ToTimeSpan(
    this ResolutionModel resolution,
    int multiplier,
    DateTimeOffset timestamp)
  {
    return resolution switch
    {
      ResolutionModel.Minute => TimeSpan.FromMinutes(1) * multiplier,
      ResolutionModel.Hour => TimeSpan.FromHours(1) * multiplier,
      ResolutionModel.Day => TimeSpan.FromDays(1) * multiplier,
      ResolutionModel.Week => TimeSpan.FromDays(7) * multiplier,
      ResolutionModel.Month => timestamp.GetMonthRange() switch
      {
        (DateTimeOffset start, DateTimeOffset end) => (end - start) * multiplier
      },
      ResolutionModel.Year => TimeSpan.FromDays(365) * multiplier,
      _ => throw new ArgumentOutOfRangeException(
        nameof(resolution),
        resolution,
        null)
    };
  }
}
