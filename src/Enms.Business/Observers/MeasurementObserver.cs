using Enms.Business.Observers.Abstractions;
using Enms.Business.Observers.EventArgs;

namespace Enms.Business.Observers;

public class MeasurementObserver : IMeasurementPublisher, IMeasurementSubscriber
{
  public void PublishPush(MeasurementPushEventArgs eventArgs)
  {
    OnPush?.Invoke(this, eventArgs);
  }

  public void SubscribePush(
    EventHandler<MeasurementPushEventArgs> handler)
  {
    OnPush += handler;
  }

  public void UnsubscribePush(
    EventHandler<MeasurementPushEventArgs> handler)
  {
    OnPush -= handler;
  }

  public event EventHandler<MeasurementPushEventArgs>? OnPush;
}
