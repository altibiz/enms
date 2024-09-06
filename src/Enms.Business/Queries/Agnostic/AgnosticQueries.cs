using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Concurrency;
using Enms.Data.Entities.Abstractions;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

// FIXME: Operation is not valid due to the current state of the object
// when ordering by primary key

public class AgnosticQueries(
  DataDbContextMutex mutex,
  IServiceProvider serviceProvider) : IQueries
{
  public async Task<T?> ReadSingle<T>(string id)
    where T : IIdentifiable
  {
    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(typeof(T)))
      ?? throw new InvalidOperationException(
        $"No model entity converter found for model {typeof(T)}");

    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IIdentifiableEntity>
      ?? throw new InvalidOperationException();

    var item = await queryable
      .Where(
        context.PrimaryKeyEquals(modelEntityConverter.EntityType(), id))
      .FirstOrDefaultAsync();

    return item is null
      ? default
      : (T)modelEntityConverter.ToModel((item as IIdentifiableEntity)!);
  }

  public async Task<T?> ReadSingleDynamic<T>(string id)
  {
    if (!typeof(T).IsAssignableTo(typeof(IIdentifiable)))
    {
      throw new InvalidOperationException(
        $"{typeof(T)} is not identifiable");
    }

    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(typeof(T)))
      ?? throw new InvalidOperationException(
        $"No model entity converter found for model {typeof(T)}");

    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IIdentifiableEntity>
      ?? throw new InvalidOperationException();

    var item = await queryable
      .Where(
        context.PrimaryKeyEquals(modelEntityConverter.EntityType(), id))
      .FirstOrDefaultAsync();

    return item is null
      ? default
      : (T)modelEntityConverter.ToModel((item as IIdentifiableEntity)!);
  }

  public async Task<PaginatedList<T>> Read<T>(
    string? whereClause = default,
    string? orderByDescClause = default,
    string? orderByAscClause = default,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : IModel
  {
    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(typeof(T)))
      ?? throw new InvalidOperationException(
        $"No model entity converter found for model {typeof(T)}");

    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IEntity>
      ?? throw new InvalidOperationException();

    var filtered = whereClause is not null
      ? queryable.WhereDynamic(whereClause)
      : queryable;

    var ordered = filtered;
    if (orderByDescClause is null && orderByAscClause is null)
    {
      ordered = filtered.OrderBy(context
        .PrimaryKeyOf(modelEntityConverter.EntityType()))
        as IQueryable<IEntity>
        ?? throw new InvalidOperationException();
    }
    else
    {
      var orderByDesc = orderByDescClause is not null
        ? filtered.OrderByDescendingDynamic(orderByDescClause)
        : filtered;
      ordered = orderByAscClause is not null
        ? orderByDesc.OrderByDescendingDynamic(orderByAscClause)
        : orderByDesc;
    }

    var count = await filtered.CountAsync();

    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    return items
      .Select(modelEntityConverter.ToModel)
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
  {
    if (!typeof(T).IsAssignableTo(typeof(IModel)))
    {
      throw new InvalidOperationException(
        $"{typeof(T)} is not a model");
    }

    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(typeof(T)))
      ?? throw new InvalidOperationException(
        $"No model entity converter found for model {typeof(T)}");

    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IEntity>
      ?? throw new InvalidOperationException();

    var filtered = whereClause is not null
      ? queryable.WhereDynamic(whereClause)
      : queryable;

    var ordered = filtered;
    if (orderByDescClause is null && orderByAscClause is null)
    {
      var ordering = context
        .PrimaryKeyOf(modelEntityConverter.EntityType());
      ordered = filtered.OrderBy(ordering)
        as IQueryable<IEntity>
        ?? throw new InvalidOperationException();
    }
    else
    {
      var orderByDesc = orderByDescClause is not null
        ? filtered.OrderByDescendingDynamic(orderByDescClause)
        : filtered;
      ordered = orderByAscClause is not null
        ? orderByDesc.OrderByDescendingDynamic(orderByAscClause)
        : orderByDesc;
    }

    var count = await filtered.CountAsync();

    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    return items
      .Select(modelEntityConverter.ToModel)
      .OfType<T>()
      .ToPaginatedList(count);
  }
}
