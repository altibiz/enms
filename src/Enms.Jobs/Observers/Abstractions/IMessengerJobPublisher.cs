using Enms.Jobs.Observers.EventArgs;

namespace Enms.Jobs.Observers.Abstractions;

public interface IMeterJobPublisher : IPublisher<IMeterJobSubscriber>
{
  public void PublishInactivity(MeterInactivityEventArgs eventArgs);
}
