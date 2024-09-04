namespace Enms.Jobs.Observers.Abstractions;

public interface IMeterJobSubscriber : ISubscriber<IMeterJobPublisher>
{
  public void SubscribeInactivity(EventHandler<string> handler);

  public void UnsubscribeInactivity(EventHandler<string> handler);
}
