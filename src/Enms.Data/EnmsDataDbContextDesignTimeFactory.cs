using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enms.Data;

public class
  BloggingContextFactory : IDesignTimeDbContextFactory<EnmsDataDbContext>
{
  public EnmsDataDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<EnmsDataDbContext>();
    optionsBuilder.UseTimescale(
      "Server=localhost;Port=5432;User Id=enms;Password=enms;Database=enms",
      x =>
      {
        x.MigrationsAssembly(
          typeof(EnmsDataDbContext).Assembly.GetName().Name);
        x.MigrationsHistoryTable(
          $"__{nameof(EnmsDataDbContext)}");
      });
    return new EnmsDataDbContext(optionsBuilder.Options);
  }
}
