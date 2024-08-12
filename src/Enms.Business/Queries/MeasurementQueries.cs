using Enms.Business.Conversion.Agnostic;
using Enms.Business.Extensions;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class MeasurementQueries(
  EnmsDataDbContext context,
  AgnosticModelEntityConverter modelEntityConverter
) : IQueries
{
  public async Task<PaginatedList<T>> Read<T>(
    string? whereClause,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IMeasurement
  {
    var result = await ReadDynamic(
      typeof(T),
      whereClause,
      fromDate,
      toDate,
      pageNumber,
      pageCount
    );
    return result.Items.Cast<T>().ToPaginatedList(result.TotalCount);
  }

  public async Task<List<IMeasurement>> ReadAgnostic(
    string? whereClause,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount)
  {
    var result = new List<IMeasurement>();

    foreach (var type in typeof(IMeasurement).Assembly
      .GetTypes()
      .Where(type => type.IsAssignableTo(typeof(IMeasurement))))
    {
      var model = await ReadDynamic(
        type,
        whereClause,
        fromDate,
        toDate,
        pageNumber,
        pageCount
      );
      result.AddRange(model.Items);
    }

    return result;
  }

  public async Task<PaginatedList<IMeasurement>> ReadDynamic(
    Type aggregateType,
    string? whereClause,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
  {
    var dbSetType = modelEntityConverter.EntityType(aggregateType);
    var queryable = context.GetQueryable(dbSetType)
        as IQueryable<MeasurementEntity>
      ?? throw new InvalidOperationException(
        $"No DbSet found for {dbSetType}");
    var filtered = whereClause is null
      ? queryable
      : queryable.WhereDynamic(whereClause);
    var timeFiltered = filtered
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
      .OfType<IMeasurement>()
      .ToPaginatedList(count);
  }
}
