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
      MeterId = measurement.MeterId,
      LineId = measurement.LineId,
      Timestamp = measurement.Timestamp,
      Interval = interval,
      Count = 1,
      VoltageL1AnyT0Avg_V = measurement.VoltageL1AnyT0_V,
      VoltageL2AnyT0Avg_V = measurement.VoltageL2AnyT0_V,
      VoltageL3AnyT0Avg_V = measurement.VoltageL3AnyT0_V,
      CurrentL1AnyT0Avg_A = measurement.CurrentL1AnyT0_A,
      CurrentL2AnyT0Avg_A = measurement.CurrentL2AnyT0_A,
      CurrentL3AnyT0Avg_A = measurement.CurrentL3AnyT0_A,
      ActivePowerL1NetT0Avg_W = measurement.ActivePowerL1NetT0_W,
      ActivePowerL2NetT0Avg_W = measurement.ActivePowerL2NetT0_W,
      ActivePowerL3NetT0Avg_W = measurement.ActivePowerL3NetT0_W,
      ApparentPowerL1NetT0Avg_W = measurement.ApparentPowerL1NetT0_W,
      ApparentPowerL2NetT0Avg_W = measurement.ApparentPowerL2NetT0_W,
      ApparentPowerL3NetT0Avg_W = measurement.ApparentPowerL3NetT0_W
    };
  }
}
