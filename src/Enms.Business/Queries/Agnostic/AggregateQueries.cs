using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Naming.Agnostic;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class AggregateQueries(
  IDbContextFactory<DataDbContext> factory,
  AgnosticModelEntityConverter modelEntityConverter,
  AgnosticLineNamingConvention lineNamingConvention
) : IQueries
{
  public async Task<PaginatedList<T>> Read<T>(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? lineId = null,
    string? meterId = null,
    IntervalModel? interval = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IAggregate
  {
    var result = await ReadDynamic(
      fromDate,
      toDate,
      typeof(T),
      lineId,
      meterId,
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
    string? meterId = null,
    IntervalModel? interval = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var result = new List<IAggregate>();

    if (lineId is not null && meterId is not null)
    {
      var aggregateType = lineNamingConvention
        .AggregateTypeForLineAndMeterId(lineId, meterId);
      var aggregates = await ReadDynamic(
        fromDate,
        toDate,
        aggregateType,
        lineId,
        meterId,
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
        .Where(
          type =>
            !type.IsGenericType &&
            !type.IsAbstract &&
            type.IsAssignableTo(typeof(IAggregate))))
      {
        var model = await ReadDynamic(
          fromDate,
          toDate,
          type,
          lineId,
          meterId,
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
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    Type? aggregateType = null,
    string? lineId = null,
    string? meterId = null,
    IntervalModel? interval = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
  {
    using var context = await factory.CreateDbContextAsync();

    if (aggregateType is null)
    {
      aggregateType = lineNamingConvention
        .AggregateTypeForLineAndMeterId(
          lineId ?? throw new ArgumentNullException(nameof(lineId)),
          meterId ?? throw new ArgumentNullException(nameof(meterId)));
    }

    var dbSetType = modelEntityConverter.EntityType(aggregateType);
    var queryable = context.GetQueryable(dbSetType)
        as IQueryable<AggregateEntity>
      ?? throw new InvalidOperationException(
        $"No DbSet found for {dbSetType}");

    var whereFiltered = whereClause is null
      ? queryable
      : queryable.WhereDynamic(whereClause);

    var intervalEntity = interval?.ToEntity();
    var intervalFiltered = intervalEntity is null
      ? whereFiltered
      : whereFiltered.Where(aggregate => aggregate.Interval == intervalEntity);

    var timeFiltered = intervalFiltered
      .Where(aggregate => aggregate.Timestamp >= fromDate)
      .Where(aggregate => aggregate.Timestamp < toDate);

    var filtered = lineId is { } nonNullLineId
      ? timeFiltered.Where(aggregate => aggregate.LineId == nonNullLineId)
      : timeFiltered;

    filtered = meterId is { } nonNullMeterId
      ? filtered.Where(aggregate => aggregate.MeterId == nonNullMeterId)
      : filtered;

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
