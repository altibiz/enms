using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models.Abstractions;
using Enms.Business.Naming.Agnostic;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Concurrency;
using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class MeasurementQueries(
  DataDbContextMutex mutex,
  AgnosticModelEntityConverter modelEntityConverter,
  AgnosticLineNamingConvention lineNamingConvention
) : IQueries
{
  public async Task<PaginatedList<T>> Read<T>(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? lineId = null,
    string? meterId = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.MeasurementPageCount
  )
    where T : class, IMeasurement
  {
    var result = await ReadDynamic(
      fromDate,
      toDate,
      typeof(T),
      lineId,
      meterId,
      whereClause,
      pageNumber,
      pageCount
    );
    return result.Items.Cast<T>().ToPaginatedList(result.TotalCount);
  }

  public async Task<List<IMeasurement>> ReadAgnostic(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    string? lineId = null,
    string? meterId = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.MeasurementPageCount
  )
  {
    var result = new List<IMeasurement>();

    if (lineId is not null && meterId is not null)
    {
      var measurementType = lineNamingConvention
        .MeasurementTypeForLineAndMeterId(lineId, meterId);
      var measurements = await ReadDynamic(
        fromDate,
        toDate,
        measurementType,
        lineId,
        meterId,
        whereClause,
        pageNumber,
        pageCount
      );
      result.AddRange(measurements.Items);
    }
    else
    {
      foreach (var type in typeof(IMeasurement).Assembly
        .GetTypes()
        .Where(
          type =>
            !type.IsGenericType &&
            !type.IsAbstract &&
            type.IsAssignableTo(typeof(IMeasurement))))
      {
        var measurements = await ReadDynamic(
          fromDate,
          toDate,
          type,
          lineId,
          meterId,
          whereClause,
          pageNumber,
          pageCount
        );
        result.AddRange(measurements.Items);
      }
    }

    return result;
  }

  public async Task<PaginatedList<IMeasurement>> ReadDynamic(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    Type? measurementType = null,
    string? lineId = null,
    string? meterId = null,
    string? whereClause = null,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.MeasurementPageCount
  )
  {
    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    if (measurementType is null)
    {
      measurementType = lineNamingConvention
        .MeasurementTypeForLineAndMeterId(
          lineId ?? throw new ArgumentNullException(nameof(lineId)),
          meterId ?? throw new ArgumentNullException(nameof(meterId)));
    }

    var dbSetType = modelEntityConverter.EntityType(measurementType);
    var queryable = context.GetQueryable(dbSetType)
        as IQueryable<MeasurementEntity>
      ?? throw new InvalidOperationException(
        $"No DbSet found for {dbSetType}");

    var whereFiltered = whereClause is null
      ? queryable
      : queryable.WhereDynamic(whereClause);

    var timeFiltered = whereFiltered
      .Where(aggregate => aggregate.Timestamp >= fromDate)
      .Where(aggregate => aggregate.Timestamp < toDate);

    var filtered = lineId is null
      ? timeFiltered
      : timeFiltered.Where(
        aggregate =>
          aggregate.LineId == lineId && aggregate.MeterId == meterId);

    var ordered = filtered
      .OrderBy(aggregate => aggregate.Timestamp);

    var count = await filtered.CountAsync();

    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    return items
      .Select(modelEntityConverter.ToModel)
      .OfType<IMeasurement>()
      .ToPaginatedList(count);
  }
}
