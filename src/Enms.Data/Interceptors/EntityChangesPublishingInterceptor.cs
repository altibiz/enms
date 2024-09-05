using Enms.Data.Entities.Abstractions;
using Enms.Data.Observers.Abstractions;
using Enms.Data.Observers.EventArgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enms.Data.Interceptors;

public class EntityChangesPublishingInterceptor(
  IServiceProvider serviceProvider)
  : ServedSaveChangesInterceptor(serviceProvider)
{
  private readonly List<EntityChangesEntry> _entries = new();

  public override int Order
  {
    get { return 0; }
  }

  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
  {
    var context = eventData.Context;
    if (context is null)
    {
      return await base.SavingChangesAsync(
        eventData, result, cancellationToken);
    }

    context.ChangeTracker.DetectChanges();
    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.Entity is not IEntity entity)
      {
        continue;
      }

      if (entry.State is not EntityState.Added
        or EntityState.Modified
        or EntityState.Deleted)
      {
        continue;
      }

      _entries.Add(
        new EntityChangesEntry(
          entry.State switch
          {
            EntityState.Added => EntityChanges.Added,
            EntityState.Modified => EntityChanges.Modified,
            EntityState.Deleted => EntityChanges.Deleted,
            _ => throw new NotImplementedException()
          },
          entity));
    }

    var publisher = serviceProvider
      .GetRequiredService<IEntityChangesPublisher>();

    publisher.PublishEntitiesChanging(
      new EntitiesChangingEventArgs
      {
        Entities = _entries
          .Select(
            entry => new EntityChangingRecord(
              entry.State switch
              {
                EntityChanges.Added => EntityChangingState.Adding,
                EntityChanges.Modified => EntityChangingState.Modifying,
                EntityChanges.Deleted => EntityChangingState.Removing,
                _ => throw new NotImplementedException()
              },
              entry.Entity
            )
          )
          .ToList()
      });

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

    publisher.PublishEntitiesChanged(
      new EntitiesChangedEventArgs
      {
        Entities = _entries
          .Select(
            entry => new EntityChangedEntry(
              entry.State switch
              {
                EntityChanges.Added => EntityChangedState.Added,
                EntityChanges.Modified => EntityChangedState.Modified,
                EntityChanges.Deleted => EntityChangedState.Removed,
                _ => throw new NotImplementedException()
              },
              entry.Entity
            )
          )
          .ToList()
      });

    return base.SavedChanges(eventData, result);
  }

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
