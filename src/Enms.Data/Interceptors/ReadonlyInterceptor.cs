using Enms.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enms.Data.Interceptors;

public class ReadonlyInterceptor : ServedSaveChangesInterceptor
{
  public ReadonlyInterceptor(IServiceProvider serviceProvider)
    : base(serviceProvider)
  {
  }

  public override int Order
  {
    get { return -100; }
  }

  public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result
  )
  {
    PreventReadonlyModifications(eventData);
    return base.SavingChanges(eventData, result);
  }

  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default
  )
  {
    PreventReadonlyModifications(eventData);
    return await base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  public void PreventReadonlyModifications(
    DbContextEventData eventData
  )
  {
    if (eventData.Context is null)
    {
      return;
    }

    eventData.Context.ChangeTracker.DetectChanges();
    var entries = eventData.Context.ChangeTracker
      .Entries<IReadonlyEntity>()
      .ToList();

    if (entries.Find(
        entry =>
          entry.State is EntityState.Modified or EntityState.Deleted) is
      { } entry)
    {
      throw new InvalidOperationException(
        $"Cannot modify readonly entity {entry.Entity.GetType().Name}.");
    }
  }
}
