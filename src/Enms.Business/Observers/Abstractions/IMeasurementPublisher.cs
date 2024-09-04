using Enms.Business.Pushing.EventArgs;

namespace Enms.Business.Pushing.Abstractions;

public interface IMeasurementPublisher : IPublisher<IMeasurementSubscriber>
{
  public void PublishPush(MeasurementPushEventArgs eventArgs);
}
