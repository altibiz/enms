using System.Reflection;
using Enms.Data.Attributes;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

// TODO: remove all the applications in favor of fluent API

namespace Enms.Data;

public partial class EnmsDbContext : DbContext
{
  public EnmsDbContext(DbContextOptions<EnmsDbContext> options)
    : base(options)
  {
  }

  protected override void OnConfiguring(
    DbContextOptionsBuilder dbContextOptionsBuilder)
  {
    dbContextOptionsBuilder
      .UseLazyLoadingProxies()
      .UseSnakeCaseNamingConvention();

    base.OnConfiguring(dbContextOptionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .HasPostgresExtension("timescaledb")
      .ApplyModelConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
      .ApplyPostgresqlEnumAttributes()
      .ApplyTimescaleHypertableAttributes();

    base.OnModelCreating(modelBuilder);
  }
}
