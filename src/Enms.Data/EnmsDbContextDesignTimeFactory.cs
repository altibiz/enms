using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enms.Data;

public class BloggingContextFactory : IDesignTimeDbContextFactory<EnmsDbContext>
{
  public EnmsDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<EnmsDbContext>();
    optionsBuilder.UseTimescale(
      "Server=localhost;Port=5432;User Id=enms;Password=enms;Database=enms");
    return new EnmsDbContext(optionsBuilder.Options);
  }
}
