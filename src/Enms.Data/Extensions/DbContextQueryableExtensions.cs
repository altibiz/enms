using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Enms.Data.Extensions;

public static class DbContextQueryableExtensions
{
  public static IQueryable<object> GetQueryable(
    this DbContext context,
    Type type)
  {
    var method = typeof(DbContext)
      .GetMethods()
      .FirstOrDefault(
        m => m.Name == nameof(DbContext.Set)
          && m.IsGenericMethodDefinition
          && m.GetParameters().Length == 0)
      ?.MakeGenericMethod(type);
    return method?.Invoke(context, null) as IQueryable<object>
      ?? throw new InvalidOperationException($"No DbSet found for {type}");
  }
}