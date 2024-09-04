using Enms.Business.Observers.EventArgs;

namespace Enms.Business.Observers.Abstractions;

public interface IMeasurementSubscriber : ISubscriber<IMeasurementPublisher>
{
  public void SubscribePush(
    EventHandler<MeasurementPushEventArgs> handler);

  public void UnsubscribePush(
    EventHandler<MeasurementPushEventArgs> handler);
}
