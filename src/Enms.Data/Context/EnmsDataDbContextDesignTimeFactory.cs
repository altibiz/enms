using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enms.Data.Context;

public class
  DataDbContextDesignTimeFactory : IDesignTimeDbContextFactory<DataDbContext>
{
  public DataDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();
    optionsBuilder.UseTimescale(
      "Server=localhost;Port=5432;User Id=enms;Password=enms;Database=enms",
      x =>
      {
        x.MigrationsAssembly(
          typeof(DataDbContext).Assembly.GetName().Name);
        x.MigrationsHistoryTable(
          $"__Enms{nameof(DataDbContext)}");
      });
    return new DataDbContext(optionsBuilder.Options);
  }
}
