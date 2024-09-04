using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enms.Jobs.Context;

public class JobsDbContextDesignTimeFactory
  : IDesignTimeDbContextFactory<JobsDbContext>
{
  public JobsDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<JobsDbContext>();
    optionsBuilder
      .UseNpgsql(
        "Server=localhost;Port=5432;User Id=enms;Password=enms;Database=enms;",
        x =>
        {
          x.MigrationsAssembly(
            typeof(JobsDbContext).Assembly.GetName().Name);
          x.MigrationsHistoryTable(
            $"__Enms{nameof(JobsDbContext)}");
        });
    return new JobsDbContext(optionsBuilder.Options);
  }
}
