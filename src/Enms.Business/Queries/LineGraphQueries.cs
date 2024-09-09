using System.Linq.Expressions;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Naming.Agnostic;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Concurrency;
using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Enms.Business.Queries;

public class LineGraphQueries(
  DataDbContextMutex mutex,
  AgnosticModelEntityConverter modelEntityConverter,
  AgnosticLineNamingConvention lineNamingConvention
) : IQueries
{
  public async Task<PaginatedList<IMeasurement>> Read(
    IEnumerable<ILine> lines,
    ResolutionModel resolution,
    int multiplier,
    DateTimeOffset fromDate = default,
    DateTimeOffset toDate = default,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.MeasurementPageCount
  )
  {
    var now = DateTimeOffset.UtcNow;
    toDate = toDate == default ? now : toDate;
    var timeSpan = resolution.ToTimeSpan(multiplier, toDate);
    fromDate = fromDate == default ? toDate.Subtract(timeSpan) : fromDate;

    var appropriateInterval = QueryConstants
      .AppropriateInterval(timeSpan, fromDate)
      ?.ToEntity();
    var isAggregate = appropriateInterval is { };

    var modelTypes = lines
      .GroupBy(line => isAggregate
        ? lineNamingConvention.AggregateTypeForLineAndMeterId(
            meterId: line.MeterId,
            lineId: line.LineId
          )
        : lineNamingConvention.MeasurementTypeForLineAndMeterId(
            meterId: line.MeterId,
            lineId: line.LineId
          ));

    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    List<IMeasurement> items;
    int count;
    if (isAggregate)
    {
      var futureCounts = new List<QueryDeferred<int>>();
      var futureItems = new List<QueryFutureEnumerable<AggregateEntity>>();

      foreach (var modelTypeLines in modelTypes)
      {
        var modelType = modelTypeLines.Key;
        var lineIds = modelTypeLines
          .Select(line => line.Id)
          .ToList();
        var entityType = modelEntityConverter.EntityType(modelType);

        var queryable = context
          .GetQueryable(modelEntityConverter.EntityType(entityType))
          as IQueryable<AggregateEntity>
          ?? throw new InvalidOperationException(
            $"No DbSet found for {entityType}");

        var parameter = Expression.Parameter(typeof(AggregateEntity), "entity");
        var foreignKeyExpression = Expression
          .Lambda<Func<AggregateEntity, bool>>(
            Expression.Invoke(
              context.ForeignKeyIn(
                entityType,
                nameof(AggregateEntity<LineEntity, MeterEntity>.Line),
                lineIds),
              Expression.Convert(parameter, typeof(object))),
            parameter);

        var filtered = queryable
          .Where(aggregate => aggregate.Timestamp >= fromDate)
          .Where(aggregate => aggregate.Timestamp < toDate)
          .Where(aggregate => aggregate.Interval == appropriateInterval)
          .Where(foreignKeyExpression);

        var ordered = filtered
          .OrderBy(aggregate => aggregate.Timestamp);

        var paged = ordered
          .Skip((pageNumber - 1) * pageCount)
          .Take(pageCount);

        futureCounts.Add(filtered.DeferredCount());
        futureItems.Add(paged.Future());
      }

      count = (await Task
        .WhenAll(futureCounts
          .Select(x => x
            .FutureValue()
            .ValueAsync())))
        .DefaultIfEmpty(0)
        .Sum();
      items = (await Task
        .WhenAll(futureItems
          .Select(x => x
            .ToListAsync())))
        .SelectMany(x => x
          .Select(modelEntityConverter.ToModel)
          .OfType<IMeasurement>())
        .ToList();
    }
    else
    {
      var futureCounts = new List<QueryDeferred<int>>();
      var futureItems = new List<QueryFutureEnumerable<MeasurementEntity>>();

      foreach (var entityTypeLines in modelTypes)
      {
        var modelType = entityTypeLines.Key;
        var lineIds = entityTypeLines
          .Select(line => line.Id)
          .ToList();
        var entityType = modelEntityConverter.EntityType(modelType);

        var queryable = context.GetQueryable(entityType)
          as IQueryable<MeasurementEntity>
          ?? throw new InvalidOperationException(
            $"No DbSet found for {entityType}");

        var parameter = Expression.Parameter(typeof(MeasurementEntity), "entity");
        var foreignKeyExpression = Expression
          .Lambda<Func<MeasurementEntity, bool>>(
            Expression.Invoke(
              context.ForeignKeyIn(
                entityType,
                nameof(MeasurementEntity<LineEntity, MeterEntity>.Line),
                lineIds),
              Expression.Convert(parameter, typeof(object))),
            parameter);

        var filtered = queryable
          .Where(measurement => measurement.Timestamp >= fromDate)
          .Where(measurement => measurement.Timestamp < toDate)
          .Where(foreignKeyExpression);

        var ordered = filtered
          .OrderBy(measurement => measurement.Timestamp);

        var paged = ordered
          .Skip((pageNumber - 1) * pageCount)
          .Take(pageCount);

        futureCounts.Add(filtered.DeferredCount());
        futureItems.Add(paged.Future());
      }

      count = (await Task
        .WhenAll(futureCounts
          .Select(x => x
            .FutureValue()
            .ValueAsync())))
        .DefaultIfEmpty(0)
        .Sum();

      items = (await Task
        .WhenAll(futureItems
          .Select(x => x
            .ToListAsync())))
        .SelectMany(x => x
          .Select(modelEntityConverter.ToModel)
          .OfType<IMeasurement>())
        .ToList();
    }

    return new PaginatedList<IMeasurement>(
      items,
      count
    );
  }
}
