using Enms.Business.Conversion;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Composite;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries;

public class DashboardQueries(
  IDbContextFactory<DataDbContext> factory,
  AgnosticModelEntityConverter modelEntityConverter
) : IQueries
{
  public async Task<List<ILine>> OperatorLines()
  {
    using var context = await factory.CreateDbContextAsync();

    var lines = await context.Lines.ToListAsync();

    return lines.Select(modelEntityConverter.ToModel<ILine>).ToList();
  }

  public async Task<NetworkUserLines?> NetworkUserLines(
    RepresentativeModel representative)
  {
    using var context = await factory.CreateDbContextAsync();

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
