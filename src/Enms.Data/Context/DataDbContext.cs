using System.Reflection;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

// TODO: remove all the applications in favor of fluent API

namespace Enms.Data.Context;

public partial class DataDbContext(DbContextOptions<DataDbContext> options)
  : DbContext(options)
{
  protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
      .UseLazyLoadingProxies()
      .UseSnakeCaseNamingConvention();

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .HasPostgresExtension("timescaledb")
      .ApplyModelConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    foreach (var relationship in modelBuilder.Model
      .GetEntityTypes()
      .SelectMany(e => e.GetForeignKeys()))
    {
      relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    base.OnModelCreating(modelBuilder);
  }
}
