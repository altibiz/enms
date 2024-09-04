using Enms.Data.Entities.Abstractions;
using Enms.Data.Observers.Abstractions;
using Enms.Data.Observers.EventArgs;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enms.Data.Interceptors;

public class EntityChangesPublishingInterceptor(IServiceProvider serviceProvider)
  : ServedSaveChangesInterceptor(serviceProvider)
{
  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
  {
    var context = eventData.Context;
    if (context is null)
    {
      return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    context.ChangeTracker.DetectChanges();
    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.Entity is not IEntity entity)
      {
        continue;
      }

      _entries.Add(new EntityChangesEntry(EntityChanges.Added, entity));
    }

    var publisher = serviceProvider
      .GetRequiredService<IEntityChangesPublisher>();

    foreach (var entry in _entries)
    {
      if (entry.State == EntityChanges.Added)
      {
        publisher.PublishEntityAdding(new EntityAddingEventArgs { Entity = entry.Entity });
      }

      if (entry.State == EntityChanges.Modified)
      {
        publisher.PublishEntityModifying(new EntityModifyingEventArgs { Entity = entry.Entity });
      }

      if (entry.State == EntityChanges.Deleted)
      {
        publisher.PublishEntityRemoving(new EntityRemovingEventArgs { Entity = entry.Entity });
      }
    }

    return await base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  public override int SavedChanges(
    SaveChangesCompletedEventData eventData,
    int result)
  {
    var context = eventData.Context;
    if (context is null)
    {
      return base.SavedChanges(eventData, result);
    }

    var publisher = serviceProvider
      .GetRequiredService<IEntityChangesPublisher>();

    foreach (var entry in _entries)
    {
      if (entry.State == EntityChanges.Added)
      {
        publisher.PublishEntityAdded(new EntityAddedEventArgs { Entity = entry.Entity });
      }

      if (entry.State == EntityChanges.Modified)
      {
        publisher.PublishEntityModified(new EntityModifiedEventArgs { Entity = entry.Entity });
      }

      if (entry.State == EntityChanges.Deleted)
      {
        publisher.PublishEntityRemoved(new EntityRemovedEventArgs { Entity = entry.Entity });
      }
    }

    return base.SavedChanges(eventData, result);
  }

  private readonly List<EntityChangesEntry> _entries = new();

  private enum EntityChanges
  {
    Added,
    Modified,
    Deleted
  }

  private sealed record EntityChangesEntry(
    EntityChanges State,
    IEntity Entity
  );
}
