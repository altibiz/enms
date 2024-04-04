using Enms.Business.Conversion.Abstractions;
using Enms.Business.Extensions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class EnmsEventQueries : IEnmsQueries
{
  private readonly EnmsDataDbContext _context;

  private readonly IServiceProvider _serviceProvider;

  public EnmsEventQueries(
    EnmsDataDbContext context,
    IServiceProvider serviceProvider
  )
  {
    _context = context;
    _serviceProvider = serviceProvider;
  }

  public async Task<PaginatedList<T>> Read<T>(
    IEnumerable<string> whereClauses,
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IEvent
  {
    var modelEntityConverter = _serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToModel(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for {typeof(T)}");
    var queryable = _context.GetDbSet(typeof(T)) as IQueryable<EventEntity>
      ?? throw new InvalidOperationException();
    var filtered = whereClauses.Aggregate(
      queryable,
      (current, clause) => current.WhereDynamic(clause));
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
      .OfType<T>()
      .ToPaginatedList(count);
  }
}
