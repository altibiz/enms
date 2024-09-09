using Enms.Business.Conversion;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Composite;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Concurrency;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries;

public class DashboardQueries(
  DataDbContextMutex mutex,
  AgnosticModelEntityConverter modelEntityConverter
) : IQueries
{
  public async Task<List<ILine>> OperatorLines()
  {
    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var lines = await context.Lines.ToListAsync();

    return lines.Select(modelEntityConverter.ToModel<ILine>).ToList();
  }

  public async Task<NetworkUserLines?> NetworkUserLines(
    RepresentativeModel representative)
  {
    using var @lock = await mutex.LockAsync();
    var context = @lock.Context;

    var networkUserLines = await context.NetworkUsers
      .Where(networkUser => networkUser.Id == representative.Id)
      .Include(networkUser => networkUser.Lines)
      .FirstOrDefaultAsync();

    return networkUserLines is null
      ? null
      : new(networkUserLines.ToModel(),
        networkUserLines.Lines
          .Select(modelEntityConverter.ToModel<ILine>)
          .ToList());
  }
}
