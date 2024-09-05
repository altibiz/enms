using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Enms.Data.Entities.Enums;

public enum PhaseEntity
{
  L1,
  L2,
  L3
}

public class PhaseEntityTypeConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<PhaseEntity>();
  }
}

public class PhaseEntityNpgsqlDataSourceConfiguration : INpgsqlDataSourceConfiguration
{
  public void Configure(NpgsqlDataSourceBuilder builder)
  {
    builder.MapEnum<PhaseEntity>();
  }
}
