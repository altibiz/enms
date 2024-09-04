using Enms.Data.Observers.EventArgs;

namespace Enms.Data.Observers.Abstractions;

public interface IEntityChangesPublisher : IPublisher<IEntityChangesSubscriber>
{
  public void PublishEntitiesChanging(EntitiesChangingEventArgs eventArgs);

  public void PublishEntitiesChanged(EntitiesChangedEventArgs eventArgs);
}
