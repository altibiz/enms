using Enms.Business.Observers.EventArgs;

namespace Enms.Business.Observers.Abstractions;

public interface IMeasurementPublisher : IPublisher<IMeasurementSubscriber>
{
  public void PublishPush(MeasurementPushEventArgs eventArgs);
}
