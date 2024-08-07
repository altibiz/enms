using Enms.Business.Conversion.Abstractions;
using Enms.Business.Extensions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class AgnosticQueries(
  EnmsDataDbContext context,
  IServiceProvider serviceProvider) : IQueries
{
  public async Task<T?> ReadSingle<T>(string id)
    where T : class, IModel
  {
    var modelEntityConverter = serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToModel(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for {typeof(T)}");
    var queryable = context.GetDbSet(typeof(T))
        as IQueryable<AuditableEntity>
      ?? throw new InvalidOperationException();
    var item = await queryable.WithId(id).FirstOrDefaultAsync();
    return item is null ? null : modelEntityConverter.ToModel(item) as T;
  }

  public async Task<PaginatedList<T>> Read<T>(
    Func<T, bool>? whereClause = default,
    Func<T, object>? orderByDescClause = default,
    Func<T, object>? orderByAscClause = default,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IModel
  {
    var modelEntityConverter = serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToModel(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for {typeof(T)}");
    var queryable = context.GetDbSet(typeof(T))
        as IQueryable<AuditableEntity>
      ?? throw new InvalidOperationException();
    var filtered = whereClause is not null
      ? queryable.Where(x => whereClause((x as T)!))
      : queryable;
    var count = await filtered.CountAsync();
    var orderByDesc = orderByDescClause is not null
      ? queryable.OrderByDescending(x => orderByDescClause((x as T)!))
      : queryable;
    var orderByAsc = orderByAscClause is not null
      ? orderByDesc.OrderByDescending(x => orderByAscClause((x as T)!))
      : orderByDesc;
    var items = await orderByAsc
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();
    return items
      .Select(item => modelEntityConverter.ToModel(item))
      .OfType<T>()
      .ToPaginatedList(count);
  }

  public async Task<PaginatedList<T>> ReadDynamic<T>(
    string? whereClause = default,
    string? orderByDescClause = default,
    string? orderByAscClause = default,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IModel
  {
    var modelEntityConverter = serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToModel(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for {typeof(T)}");
    var queryable = context.GetDbSet(typeof(T))
        as IQueryable<AuditableEntity>
      ?? throw new InvalidOperationException();
    var filtered = whereClause is not null
      ? queryable.WhereDynamic(whereClause)
      : queryable;
    var count = await filtered.CountAsync();
    var orderByDesc = orderByDescClause is not null
      ? queryable.OrderByDescendingDynamic(orderByDescClause)
      : queryable;
    var orderByAsc = orderByAscClause is not null
      ? orderByDesc.OrderByDescendingDynamic(orderByAscClause)
      : orderByDesc;
    var items = await orderByAsc
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();
    return items
      .Select(item => modelEntityConverter.ToModel(item))
      .OfType<T>()
      .ToPaginatedList(count);
  }
}
