using Enms.Data.Entities.Abstractions;
using Enms.Data.Observers.Abstractions;
using Enms.Data.Observers.EventArgs;
using Humanizer;

namespace Enms.Data.Observers;

public class EntityChangesObserver
  : IEntityChangesPublisher, IEntityChangesSubscriber
{
  private event EventHandler<EntityAddedEventArgs>? OnEntityAdded;

  private event EventHandler<EntityAddingEventArgs>? OnEntityAdding;

  private event EventHandler<EntityModifiedEventArgs>? OnEntityModified;

  private event EventHandler<EntityModifyingEventArgs>? OnEntityModifying;

  private event EventHandler<EntityRemovedEventArgs>? OnEntityRemoved;

  private event EventHandler<EntityRemovingEventArgs>? OnEntityRemoving;

  public void PublishEntityAdded(EntityAddedEventArgs eventArgs)
  {
    OnEntityAdded?.Invoke(this, eventArgs);
  }

  public void PublishEntityAdding(EntityAddingEventArgs eventArgs)
  {
    OnEntityAdding?.Invoke(this, eventArgs);
  }

  public void PublishEntityModified(EntityModifiedEventArgs eventArgs)
  {
    OnEntityModified?.Invoke(this, eventArgs);
  }

  public void PublishEntityModifying(EntityModifyingEventArgs eventArgs)
  {
    OnEntityModifying?.Invoke(this, eventArgs);
  }

  public void PublishEntityRemoved(EntityRemovedEventArgs eventArgs)
  {
    OnEntityRemoved?.Invoke(this, eventArgs);
  }

  public void PublishEntityRemoving(EntityRemovingEventArgs eventArgs)
  {
    OnEntityRemoving?.Invoke(this, eventArgs);
  }

  public void SubscribeEntityAdded(EventHandler<EntityAddedEventArgs> handler)
  {
    OnEntityAdded += handler;
  }

  public void SubscribeEntityAdding(EventHandler<EntityAddingEventArgs> handler)
  {
    OnEntityAdding += handler;
  }

  public void SubscribeEntityModified(EventHandler<EntityModifiedEventArgs> handler)
  {
    OnEntityModified += handler;
  }

  public void SubscribeEntityModifying(EventHandler<EntityModifyingEventArgs> handler)
  {
    OnEntityModifying += handler;
  }

  public void SubscribeEntityRemoved(EventHandler<EntityRemovedEventArgs> handler)
  {
    OnEntityRemoved += handler;
  }

  public void SubscribeEntityRemoving(EventHandler<EntityRemovingEventArgs> handler)
  {
    OnEntityRemoving += handler;
  }

  public void UnsubscribeEntityAdded(EventHandler<EntityAddedEventArgs> handler)
  {
    OnEntityAdded -= handler;
  }

  public void UnsubscribeEntityAdding(EventHandler<EntityAddingEventArgs> handler)
  {
    OnEntityAdding -= handler;
  }

  public void UnsubscribeEntityModified(EventHandler<EntityModifiedEventArgs> handler)
  {
    OnEntityModified -= handler;
  }

  public void UnsubscribeEntityModifying(EventHandler<EntityModifyingEventArgs> handler)
  {
    OnEntityModifying -= handler;
  }

  public void UnsubscribeEntityRemoved(EventHandler<EntityRemovedEventArgs> handler)
  {
    OnEntityRemoved -= handler;
  }

  public void UnsubscribeEntityRemoving(EventHandler<EntityRemovingEventArgs> handler)
  {
    OnEntityRemoving -= handler;
  }
}
