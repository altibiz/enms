using Microsoft.EntityFrameworkCore.Diagnostics;

// TODO: implement cascading soft delete

namespace Enms.Business.Interceptors;

public class CascadingSoftDeleteInterceptor : ServedSaveChangesInterceptor
{
  public CascadingSoftDeleteInterceptor(IServiceProvider serviceProvider)
    : base(serviceProvider)
  {
  }

  public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result)
  {
    AddCascadingSoftDeletes(eventData);
    return base.SavingChanges(eventData, result);
  }

  private static void AddCascadingSoftDeletes(DbContextEventData _)
  {
    // TODO: implement
  }
}
