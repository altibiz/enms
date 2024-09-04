using Enms.Data.Observers.EventArgs;

namespace Enms.Data.Observers.Abstractions;

public interface IEntityChangesSubscriber : ISubscriber<IEntityChangesPublisher>
{
  public void SubscribeEntityAdded(
    EventHandler<EntityAddedEventArgs> handler);

  public void UnsubscribeEntityAdded(
    EventHandler<EntityAddedEventArgs> handler);

  public void SubscribeEntityAdding(
    EventHandler<EntityAddingEventArgs> handler);

  public void UnsubscribeEntityAdding(
    EventHandler<EntityAddingEventArgs> handler);

  public void SubscribeEntityModified(
    EventHandler<EntityModifiedEventArgs> handler);

  public void UnsubscribeEntityModified(
    EventHandler<EntityModifiedEventArgs> handler);

  public void SubscribeEntityModifying(
    EventHandler<EntityModifyingEventArgs> handler);

  public void UnsubscribeEntityModifying(
    EventHandler<EntityModifyingEventArgs> handler);

  public void SubscribeEntityRemoved(
    EventHandler<EntityRemovedEventArgs> handler);

  public void UnsubscribeEntityRemoved(
    EventHandler<EntityRemovedEventArgs> handler);

  public void SubscribeEntityRemoving(
    EventHandler<EntityRemovingEventArgs> handler);

  public void UnsubscribeEntityRemoving(
    EventHandler<EntityRemovingEventArgs> handler);
}
