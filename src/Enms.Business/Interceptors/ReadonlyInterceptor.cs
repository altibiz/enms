using Enms.Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

// TODO: check if this is the right way to do it

namespace Enms.Business.Interceptors;

public class ReadonlyInterceptor : ServedSaveChangesInterceptor
{
  public ReadonlyInterceptor(IServiceProvider serviceProvider)
    : base(serviceProvider)
  {
  }

  public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result
  )
  {
    return PreventReadonlyModifications(eventData, result);
  }

  public InterceptionResult<int> PreventReadonlyModifications(
    DbContextEventData eventData,
    InterceptionResult<int> result
  )
  {
    if (eventData.Context is null)
    {
      return result;
    }

    var entries = eventData.Context.ChangeTracker
      .Entries<IReadonlyEntity>()
      .ToList();

    foreach (var @readonly in entries)
    {
      if (@readonly.State is EntityState.Modified or EntityState.Deleted)
      {
        throw new InvalidOperationException("Cannot modify readonly entity.");
      }
    }

    return result;
  }
}
