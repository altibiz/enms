using System.Reflection;
using Enms.Data.Attributes;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

// TODO: remove all the applications in favor of fluent API

namespace Enms.Data;

public partial class EnmsDataDbContext : DbContext
{
  public EnmsDataDbContext(DbContextOptions<EnmsDataDbContext> options)
    : base(options)
  {
  }

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
      .ApplyModelConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
      .ApplyPostgresqlEnumAttributes()
      .ApplyTimescaleHypertableAttributes();

    base.OnModelCreating(modelBuilder);
  }
}
