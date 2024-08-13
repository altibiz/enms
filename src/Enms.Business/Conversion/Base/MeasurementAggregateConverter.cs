using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Conversion.Base;

public abstract class
  MeasurementAggregateConverter<TMeasurement, TAggregate> :
  IMeasurementAggregateConverter
  where TAggregate : class, IAggregate
  where TMeasurement : class, IMeasurement
{
  public bool CanConvertToAggregate(Type measurement)
  {
    return measurement.IsAssignableTo(typeof(TMeasurement));
  }

  public IAggregate ToAggregate(
    IMeasurement measurement,
    IntervalModel interval)
  {
    return ToAggregate(
      measurement as TMeasurement
      ?? throw new ArgumentNullException(nameof(measurement)),
      interval
    );
  }

  protected abstract TAggregate ToAggregate(
    TMeasurement measurement,
    IntervalModel interval);
}
