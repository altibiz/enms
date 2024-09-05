using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Enms.Data.Entities.Enums;

public enum RoleEntity
{
  OperatorRepresentative,

  UserRepresentative
}

public class RoleEntityTypeConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<RoleEntity>();
  }
}

public class RoleEntityNpgsqlDataSourceConfiguration : INpgsqlDataSourceConfiguration
{
  public void Configure(NpgsqlDataSourceBuilder builder)
  {
    builder.MapEnum<RoleEntity>();
  }
}
