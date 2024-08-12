using Enms.Business.Models.Abstractions;

namespace Enms.Business.Pushing.Abstractions;

public class PublishEventArgs(
  IReadOnlyList<IMeasurement> measurements,
  IReadOnlyList<IAggregate> aggregates) : EventArgs
{
  public IReadOnlyList<IMeasurement> Measurements { get; } = measurements;

  public IReadOnlyList<IAggregate> Aggregates { get; } = aggregates;
}

public interface IMeasurementSubscriber
{
  public event EventHandler<PublishEventArgs>? OnBeforePublish;

  public event EventHandler<PublishEventArgs>? OnAfterPublish;
}
