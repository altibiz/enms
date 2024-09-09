using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries;

public class EventQueries(
  IDbContextFactory<DataDbContext> factory,
  AgnosticModelEntityConverter modelEntityConverter
) : IQueries
{
  public async Task<PaginatedList<RepresentativeAuditEventModel>>
    ReadRepresentativeAuditEvents(
      string representativeId,
      int pageNumber = QueryConstants.StartingPage,
      int pageCount = QueryConstants.DefaultPageCount
    )
  {
    using var context = await factory.CreateDbContextAsync();

    var queryable = context.Events;

    var filtered = queryable
      .OfType<RepresentativeAuditEventEntity>()
      .Where(x => x.RepresentativeId == representativeId);

    var ordered = filtered
      .OrderByDescending(x => x.Timestamp);

    var count = await filtered.CountAsync();

    var items = await ordered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    return items
      .Select(modelEntityConverter.ToModel)
      .OfType<RepresentativeAuditEventModel>()
      .ToPaginatedList(count);
  }
}
