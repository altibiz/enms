using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Conversion.Agnostic;

public class AgnosticMeasurementAggregateConverter(
  IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public IAggregate ToAggregate(IMeasurement model, IntervalModel interval)
  {
    return _serviceProvider
        .GetServices<IMeasurementAggregateConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvertToAggregate(model.GetType()))
        ?.ToAggregate(model, interval)
      ?? throw new InvalidOperationException(
        $"No converter found for measurement {model.GetType()}.");
  }

  public TAggregate ToAggregate<TAggregate>(
    IMeasurement model,
    IntervalModel interval)
    where TAggregate : class, IAggregate
  {
    return ToAggregate(model, interval) as TAggregate
      ?? throw new InvalidOperationException(
        $"No converter found for measurement {model.GetType()}.");
  }
}
