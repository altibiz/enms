using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Enms.Data.Entities.Enums;

public enum DurationEntity
{
  Second,
  Minute,
  Hour,
  Day,
  Week,
  Month,
  Year
}

public class DurationEntityModelConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<DurationEntity>();
  }
}

public class
  DurationEntityNpgsqlDataSourceConfiguration : INpgsqlDataSourceConfiguration
{
  public void Configure(NpgsqlDataSourceBuilder builder)
  {
    builder.MapEnum<DurationEntity>();
  }
}
