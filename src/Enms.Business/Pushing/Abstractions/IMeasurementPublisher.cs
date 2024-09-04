using Enms.Business.Models.Abstractions;

namespace Enms.Business.Pushing.Abstractions;

public interface IMeasurementPublisher
{
  public void BeforePush(
    IReadOnlyList<IMeasurement> measurements,
    IReadOnlyList<IAggregate> aggregates);

  public void AfterPush(
    IReadOnlyList<IMeasurement> measurements,
    IReadOnlyList<IAggregate> aggregates);
}
