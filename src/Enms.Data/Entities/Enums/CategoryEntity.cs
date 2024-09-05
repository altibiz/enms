using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Enms.Data.Entities.Enums;

public enum CategoryEntity
{
  All,
  Meter,
  MeterPush
}

public class CategoryEntityModelConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<CategoryEntity>();
  }
}

public class
  CategoryEntityNpgsqlDataSourceConfiguration : INpgsqlDataSourceConfiguration
{
  public void Configure(NpgsqlDataSourceBuilder builder)
  {
    builder.MapEnum<CategoryEntity>();
  }
}
