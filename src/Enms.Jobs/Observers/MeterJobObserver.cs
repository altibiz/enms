using Enms.Jobs.Observers.Abstractions;
using Enms.Jobs.Observers.EventArgs;

namespace Enms.Jobs.Observers;

public class MessengerJobManager :
  IMeterJobPublisher,
  IMeterJobSubscriber
{
  private EventHandler<MeterInactivityEventArgs>? OnInactivity { get; set; }

  public void PublishInactivity(MeterInactivityEventArgs eventArgs)
  {
    OnInactivity?.Invoke(this, eventArgs);
  }

  public void SubscribeInactivity(EventHandler<MeterInactivityEventArgs> handler)
  {
    OnInactivity += handler;
  }

  public void UnsubscribeInactivity(EventHandler<MeterInactivityEventArgs> handler)
  {
    OnInactivity -= handler;
  }
}
