using Enms.Business.Observers.Abstractions;
using Enms.Business.Observers.EventArgs;

namespace Enms.Business.Observers;

public class MeasurementObserver : IMeasurementPublisher, IMeasurementSubscriber
{
  public void PublishPush(MeasurementPushEventArgs eventArgs)
  {
    OnPush?.Invoke(this, eventArgs);
  }

  public void PublishUpsert(MeasurementUpsertEventArgs eventArgs)
  {
    OnUpsert?.Invoke(this, eventArgs);
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

  public void SubscribeUpsert(
    EventHandler<MeasurementUpsertEventArgs> handler)
  {
    OnUpsert += handler;
  }

  public void UnsubscribeUpsert(
    EventHandler<MeasurementUpsertEventArgs> handler)
  {
    OnUpsert -= handler;
  }

  private event EventHandler<MeasurementPushEventArgs>? OnPush;

  private event EventHandler<MeasurementUpsertEventArgs>? OnUpsert;
}
