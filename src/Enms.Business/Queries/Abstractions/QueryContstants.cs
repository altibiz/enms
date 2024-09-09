using Enms.Business.Models.Enums;

namespace Enms.Business.Queries.Abstractions;

public static class QueryConstants
{
  public const int StartingPage = 1;

  public const int DefaultPageCount = 50;

  public const int MeasurementPageCount = 10000;

  public static TimeSpan AggregateThreshold(
    IntervalModel interval,
    DateTimeOffset timestamp,
    int lineCount = 1,
    int pageCount = MeasurementPageCount
  )
  {
    var higherResolutionTimeSpan = interval.HigherResolutionTimeSpan(timestamp);
    return pageCount * higherResolutionTimeSpan / lineCount;
  }

  public static IntervalModel? AppropriateInterval(
    TimeSpan timeSpan,
    DateTimeOffset timestamp,
    int lineCount = 1,
    int pageCount = MeasurementPageCount
  )
  {
    var quarterHourThreshold = AggregateThreshold(
      IntervalModel.QuarterHour,
      timestamp,
      lineCount,
      pageCount);
    if (timeSpan < quarterHourThreshold)
    {
      return null;
    }

    var dayThreshold = AggregateThreshold(
      IntervalModel.Day,
      timestamp,
      lineCount,
      pageCount);
    if (timeSpan < dayThreshold)
    {
      return IntervalModel.QuarterHour;
    }

    var monthThreshold = AggregateThreshold(
      IntervalModel.Month,
      timestamp,
      lineCount,
      pageCount);
    if (timeSpan < monthThreshold)
    {
      return IntervalModel.Day;
    }

    return IntervalModel.Month;
  }
}
