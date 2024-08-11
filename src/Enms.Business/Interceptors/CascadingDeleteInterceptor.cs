using Enms.Business.Extensions;
using Enms.Data;
using Enms.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enms.Business.Interceptors;

public class CascadingDeleteInterceptor : ServedSaveChangesInterceptor
{
  public CascadingDeleteInterceptor(IServiceProvider serviceProvider)
    : base(serviceProvider)
  {
  }

  public override int Order
  {
    get { return 10; }
  }

  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
  {
    await AddCascadingDeletes(eventData);
    return await base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private async ValueTask AddCascadingDeletes(
    DbContextEventData eventData)
  {
    var context = eventData.Context as EnmsDataDbContext;
    if (context is null)
    {
      return;
    }

    var entries = context.ChangeTracker.Entries<IIdentifiableEntity>().ToList();

    foreach (var entry in entries.Where(e => e.State is EntityState.Deleted))
    {
      await CascadingDelete(
        eventData,
        entry
      );
    }
  }

  private async Task CascadingDelete(
    DbContextEventData eventData,
    EntityEntry<IIdentifiableEntity> entry)
  {
    var context = eventData.Context as EnmsDataDbContext;
    if (context is null)
    {
      return;
    }

    var relationships = context.Model
      .GetEntityTypes()
      .SelectMany(e => e.GetForeignKeys())
      .Where(relationship => relationship.IsRequired)
      .Where(
        relationship => relationship.PrincipalEntityType
          == entry.Metadata);

    foreach (var relationship in relationships
      .Where(relationship => !relationship.DeclaringEntityType.ClrType
        .IsAssignableTo(typeof(IReadonlyEntity)))
      .Where(relationship => relationship.DeclaringEntityType.ClrType
        .IsAssignableTo(typeof(IIdentifiableEntity))))
    {
      var declarers = await context
        .GetQueryable(relationship.DeclaringEntityType.ClrType)
        .Where(
          context.ForeignKeyEqualsAgnostic(
            relationship.PrincipalEntityType.ClrType,
            relationship.GetNavigation(true)?.Name
            ?? throw new InvalidOperationException(
              "No navigation property found"),
            entry.Entity.Id))
        .ToListAsync();

      foreach (var declaring in declarers)
      {
        var declaringEntry = context.Entry((IIdentifiableEntity)declaring);
        entry.State = EntityState.Deleted;

        await CascadingDelete(
          eventData,
          declaringEntry
        );
      }
    }
  }
}
