using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Conversion.Abstractions;

public interface IMeasurementAggregateConverter
{
  bool CanConvertToAggregate(Type measurement);

  IAggregate ToAggregate(IMeasurement measurement, IntervalModel interval);
}
