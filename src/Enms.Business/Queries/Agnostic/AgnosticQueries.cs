using Enms.Business.Conversion.Abstractions;
using Enms.Business.Extensions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Entities.Abstractions;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

// FIXME: Operation is not valid due to the current state of the object
// when ordering by primary key

public class AgnosticQueries(
  EnmsDataDbContext context,
  IServiceProvider serviceProvider) : IQueries
{
  private static readonly SemaphoreSlim Semaphore = new(1, 1);

  public async Task<T?> ReadSingle<T>(string id)
    where T : IIdentifiable
  {
    var modelEntityConverter = serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToEntity(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for model {typeof(T)}");

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IIdentifiableEntity>
      ?? throw new InvalidOperationException();

    await Semaphore.WaitAsync();

    var item = await queryable
      .Where(context.PrimaryKeyEqualsAgnostic(modelEntityConverter.EntityType(), id))
      .FirstOrDefaultAsync();

    Semaphore.Release();

    return item is null ? default : (T)modelEntityConverter.ToModel((item as IIdentifiableEntity)!);
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
          .CanConvertToEntity(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for model {typeof(T)}");

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IIdentifiableEntity>
      ?? throw new InvalidOperationException();

    await Semaphore.WaitAsync();

    var item = await queryable
      .Where(context.PrimaryKeyEqualsAgnostic(modelEntityConverter.EntityType(), id))
      .FirstOrDefaultAsync();

    Semaphore.Release();

    return item is null ? default : (T)modelEntityConverter.ToModel((item as IIdentifiableEntity)!);
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
          .CanConvertToEntity(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for model {typeof(T)}");

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IEntity>
      ?? throw new InvalidOperationException();

    var filtered = whereClause is not null
      ? queryable.WhereDynamic(whereClause)
      : queryable;

    var ordered = filtered;
    if (orderByDescClause is null && orderByAscClause is null)
    {
#pragma warning disable S125 // Sections of code should not be commented out
      // ordered = filtered.OrderBy(context
      //   .PrimaryKeyOfAgnostic(modelEntityConverter.EntityType()))
      //   as IQueryable<IEntity>
      //   ?? throw new InvalidOperationException();
#pragma warning restore S125 // Sections of code should not be commented out
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

    await Semaphore.WaitAsync();

    var count = await filtered.CountAsync();

    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    Semaphore.Release();

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
          .CanConvertToEntity(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for model {typeof(T)}");

    var queryable = context.GetQueryable(modelEntityConverter.EntityType())
        as IQueryable<IEntity>
      ?? throw new InvalidOperationException();

    var filtered = whereClause is not null
      ? queryable.WhereDynamic(whereClause)
      : queryable;

    var ordered = filtered;
    if (orderByDescClause is null && orderByAscClause is null)
    {
#pragma warning disable S125 // Sections of code should not be commented out
      // ordered = filtered.OrderBy(context
      //   .PrimaryKeyOfAgnostic(modelEntityConverter.EntityType()))
      //   as IQueryable<IEntity>
      //   ?? throw new InvalidOperationException();
#pragma warning restore S125 // Sections of code should not be commented out
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

    await Semaphore.WaitAsync();

    var count = await filtered.CountAsync();

    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    Semaphore.Release();

    return items
      .Select(modelEntityConverter.ToModel)
      .OfType<T>()
      .ToPaginatedList(count);
  }
}
