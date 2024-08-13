using Enms.Business.Models.Abstractions;

namespace Enms.Business.Pushing.Abstractions;

public class MeasurementPublishEventArgs(
  IReadOnlyList<IMeasurement> measurements,
  IReadOnlyList<IAggregate> aggregates) : EventArgs
{
  public IReadOnlyList<IMeasurement> Measurements { get; } = measurements;

  public IReadOnlyList<IAggregate> Aggregates { get; } = aggregates;
}

public interface IMeasurementSubscriber
{
  public event EventHandler<MeasurementPublishEventArgs>? OnBeforePublish;

  public event EventHandler<MeasurementPublishEventArgs>? OnAfterPublish;
}
