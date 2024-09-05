using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Enms.Data.Context;

public class
  DataDbContextDesignTimeFactory : IDesignTimeDbContextFactory<DataDbContext>
{
  public DataDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();

    var dataSourceBuilder =
    new NpgsqlDataSourceBuilder(
      "Server=localhost;Port=5432;User Id=enms;Password=enms;Database=enms");
    dataSourceBuilder.ApplyConfigurationsFromAssembly(
      typeof(DataDbContext).Assembly);

    optionsBuilder.UseTimescale(
     dataSourceBuilder.Build(),
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
