using Enms.Business.Models.Abstractions;
using Enms.Business.Pushing.Abstractions;
using Enms.Business.Pushing.EventArgs;

namespace Enms.Business.Pushing;

public class MeasurementPublisher : IMeasurementPublisher, IMeasurementSubscriber
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
