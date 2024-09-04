using Enms.Data.Observers.EventArgs;

namespace Enms.Data.Observers.Abstractions;

public interface IEntityChangesPublisher : IPublisher<IEntityChangesSubscriber>
{
  public void PublishEntityAdded(EntityAddedEventArgs eventArgs);

  public void PublishEntityAdding(EntityAddingEventArgs eventArgs);

  public void PublishEntityModified(EntityModifiedEventArgs eventArgs);

  public void PublishEntityModifying(EntityModifyingEventArgs eventArgs);

  public void PublishEntityRemoved(EntityRemovedEventArgs eventArgs);

  public void PublishEntityRemoving(EntityRemovingEventArgs eventArgs);
}
