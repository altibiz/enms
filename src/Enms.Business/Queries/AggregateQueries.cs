using Enms.Business.Conversion.Agnostic;
using Enms.Business.Extensions;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Concurrency;
using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class AggregateQueries(
  EnmsDataDbContextMutex mutex,
  AgnosticModelEntityConverter modelEntityConverter
) : IQueries
{
  public async Task<PaginatedList<T>> Read<T>(
    IntervalModel interval,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IAggregate
  {
    var result = await ReadDynamic(
      typeof(T),
      interval,
      fromDate,
      toDate,
      whereClause,
      pageNumber,
      pageCount
    );
    return result.Items.Cast<T>().ToPaginatedList(result.TotalCount);
  }

  public async Task<List<IAggregate>> ReadAgnostic(
    IntervalModel interval,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var result = new List<IAggregate>();

    foreach (var type in typeof(IAggregate).Assembly
      .GetTypes()
      .Where(type => type.IsAssignableTo(typeof(IAggregate))))
    {
      var model = await ReadDynamic(
        type,
        interval,
        fromDate,
        toDate,
        whereClause,
        pageNumber,
        pageCount
      );
      result.AddRange(model.Items);
    }

    return result;
  }

  public async Task<PaginatedList<IAggregate>> ReadDynamic(
    Type aggregateType,
    IntervalModel interval,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
  {
    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var dbSetType = modelEntityConverter.EntityType(aggregateType);
    var queryable = context.GetQueryable(dbSetType)
        as IQueryable<AggregateEntity>
      ?? throw new InvalidOperationException(
        $"No DbSet found for {dbSetType}");

    var filtered = whereClause is null
      ? queryable
      : queryable.WhereDynamic(whereClause);

    var timeFiltered = filtered
      .Where(aggregate => aggregate.Interval >= interval.ToEntity())
      .Where(aggregate => aggregate.Timestamp >= fromDate)
      .Where(aggregate => aggregate.Timestamp < toDate);

    var count = await timeFiltered.CountAsync();

    var ordered = timeFiltered
      .OrderByDescending(aggregate => aggregate.Timestamp);
    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();
    return items
      .Select(modelEntityConverter.ToModel)
      .OfType<IAggregate>()
      .ToPaginatedList(count);
  }
}
