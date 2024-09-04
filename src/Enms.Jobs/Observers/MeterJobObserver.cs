using Enms.Jobs.Observers.Abstractions;

namespace Enms.Jobs.Observers;

public class MessengerJobManager :
  IMeterJobPublisher,
  IMeterJobSubscriber
{
  private EventHandler<string>? OnInactivity { get; set; }

  public void PublishInactivity(string id)
  {
    OnInactivity?.Invoke(this, id);
  }

  public void SubscribeInactivity(EventHandler<string> handler)
  {
    OnInactivity += handler;
  }

  public void UnsubscribeInactivity(EventHandler<string> handler)
  {
    OnInactivity -= handler;
  }
}
