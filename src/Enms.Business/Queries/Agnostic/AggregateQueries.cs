using Enms.Business.Conversion.Agnostic;
using Enms.Business.Extensions;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Naming.Agnostic;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Concurrency;
using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class AggregateQueries(
  EnmsDataDbContextMutex mutex,
  AgnosticModelEntityConverter modelEntityConverter,
  AgnosticLineNamingConvention lineNamingConvention
) : IQueries
{
  public async Task<PaginatedList<T>> Read<T>(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? lineId = null,
    IntervalModel? interval = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IAggregate
  {
    var result = await ReadDynamic(
      typeof(T),
      fromDate,
      toDate,
      lineId,
      interval,
      whereClause,
      pageNumber,
      pageCount
    );
    return result.Items.Cast<T>().ToPaginatedList(result.TotalCount);
  }

  public async Task<List<IAggregate>> ReadAgnostic(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? lineId = null,
    IntervalModel? interval = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var result = new List<IAggregate>();

    if (lineId is not null)
    {
      var aggregateType = lineNamingConvention
        .AggregateTypeForLineId(lineId);
      var aggregates = await ReadDynamic(
        aggregateType,
        fromDate,
        toDate,
        lineId,
        interval,
        whereClause,
        pageNumber,
        pageCount
      );
      result.AddRange(aggregates.Items);
    }
    else
    {
      foreach (var type in typeof(IAggregate).Assembly
        .GetTypes()
        .Where(type =>
          !type.IsGenericType &&
          !type.IsAbstract &&
          type.IsAssignableTo(typeof(IAggregate))))
      {
        var model = await ReadDynamic(
          type,
          fromDate,
          toDate,
          lineId,
          interval,
          whereClause,
          pageNumber,
          pageCount
        );
        result.AddRange(model.Items);
      }
    }

    return result;
  }

  public async Task<PaginatedList<IAggregate>> ReadDynamic(
    Type aggregateType,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? lineId = null,
    IntervalModel? interval = null,
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

    var whereFiltered = whereClause is null
      ? queryable
      : queryable.WhereDynamic(whereClause);

    var timeFiltered = whereFiltered
      .Where(aggregate => aggregate.Timestamp >= fromDate)
      .Where(aggregate => aggregate.Timestamp < toDate);

    var intervalEntity = interval?.ToEntity();
    var intervalFiltered = intervalEntity is null
      ? timeFiltered
      : timeFiltered.Where(aggregate => aggregate.Interval == intervalEntity);

    var filtered = lineId is null
      ? intervalFiltered
      : intervalFiltered.Where(aggregate => aggregate.LineId == lineId);

    var ordered = filtered
      .OrderByDescending(aggregate => aggregate.Timestamp);

    var count = await filtered.CountAsync();

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
