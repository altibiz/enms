using Enms.Jobs.Observers.EventArgs;

namespace Enms.Jobs.Observers.Abstractions;

public interface IMeterJobSubscriber : ISubscriber<IMeterJobPublisher>
{
  public void SubscribeInactivity(
    EventHandler<MeterInactivityEventArgs> handler);

  public void UnsubscribeInactivity(
    EventHandler<MeterInactivityEventArgs> handler);
}
