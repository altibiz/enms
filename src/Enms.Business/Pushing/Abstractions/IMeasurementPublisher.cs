using Enms.Business.Models.Abstractions;

namespace Enms.Business.Pushing.Abstractions;

public interface IMeasurementPublisher
{
  public void BeforePublish(
    IReadOnlyList<IMeasurement> measurements,
    IReadOnlyList<IAggregate> aggregates);

  public void AfterPublish(
    IReadOnlyList<IMeasurement> measurements,
    IReadOnlyList<IAggregate> aggregates);
}
