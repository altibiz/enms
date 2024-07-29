using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;

namespace Enms.Business.Conversion;

public class EgaugeMeasurementAggregateConverter : MeasurementAggregateConverter
  <EgaugeMeasurementModel, EgaugeAggregateModel>
{
  protected override EgaugeAggregateModel ToAggregate(
    EgaugeMeasurementModel measurement,
    IntervalModel interval)
  {
    return measurement.ToAggregate(interval);
  }
}

public static class EgaugeMeasurementAggregateConverterExtensions
{
  public static EgaugeAggregateModel ToAggregate(
    this EgaugeMeasurementModel measurement,
    IntervalModel interval)
  {
    return new EgaugeAggregateModel
    {
      LineId = measurement.LineId,
      Timestamp = measurement.Timestamp,
      Interval = interval,
      Count = 1
    };
  }
}
