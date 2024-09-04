using Enms.Business.Pushing.EventArgs;

namespace Enms.Business.Pushing.Abstractions;

public interface IMeasurementSubscriber : ISubscriber<IMeasurementPublisher>
{
  public void SubscribePush(
    EventHandler<MeasurementPushEventArgs> handler);

  public void UnsubscribePush(
    EventHandler<MeasurementPushEventArgs> handler);
}
