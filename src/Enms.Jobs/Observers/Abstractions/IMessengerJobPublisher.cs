namespace Enms.Jobs.Observers.Abstractions;

public interface IMeterJobPublisher : IPublisher<IMeterJobSubscriber>
{
  public void PublishInactivity(string id);
}
