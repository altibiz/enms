using Enms.Business.Conversion.Abstractions;
using Enms.Business.Extensions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries.Agnostic;

public class EnmsAuditableQueries : IEnmsQueries
{
  private readonly EnmsDataDbContext _context;

  private readonly IServiceProvider _serviceProvider;

  public EnmsAuditableQueries(
    EnmsDataDbContext context,
    IServiceProvider serviceProvider)
  {
    _context = context;
    _serviceProvider = serviceProvider;
  }

  public async Task<T?> ReadSingle<T>(string id)
    where T : class, IAuditable
  {
    var modelEntityConverter = _serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToModel(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for {typeof(T)}");
    var queryable = _context.GetDbSet(typeof(T))
        as IQueryable<AuditableEntity>
      ?? throw new InvalidOperationException();
    var item = await queryable.WithId(id).FirstOrDefaultAsync();
    return item is null ? null : modelEntityConverter.ToModel(item) as T;
  }

  public async Task<PaginatedList<T>> Read<T>(
    IEnumerable<string> whereClauses,
    IEnumerable<string> orderByDescClauses,
    IEnumerable<string> orderByAscClauses,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
    where T : class, IAuditable
  {
    var modelEntityConverter = _serviceProvider
      .GetServices<IModelEntityConverter>()
      .FirstOrDefault(
        converter => converter
          .CanConvertToModel(typeof(T))) ?? throw new InvalidOperationException(
      $"No model entity converter found for {typeof(T)}");
    var queryable = _context.GetDbSet(typeof(T))
        as IQueryable<AuditableEntity>
      ?? throw new InvalidOperationException();
    var filtered = whereClauses.Aggregate(
      queryable,
      (current, clause) => current.WhereDynamic(clause));
    var count = await filtered.CountAsync();
    var orderedByDesc = orderByDescClauses.Aggregate(
      filtered,
      (current, clause) => current.OrderByDescendingDynamic(clause));
    var orderedByAsc = orderByAscClauses.Aggregate(
      orderedByDesc,
      (current, clause) => current.OrderByDynamic(clause));
    var items = await orderedByAsc.Skip((pageNumber - 1) * pageCount)
      .Take(pageCount).ToListAsync();
    return items
      .Select(item => modelEntityConverter.ToModel(item))
      .OfType<T>()
      .ToPaginatedList(count);
  }
}
