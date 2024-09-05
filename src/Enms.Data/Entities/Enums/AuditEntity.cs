using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Enms.Data.Entities.Enums;

public enum AuditEntity
{
  Query,
  Creation,
  Modification,
  Deletion
}

public class AuditEntityTypeConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<AuditEntity>();
  }
}

public class AuditEntityNpgsqlDataSourceConfiguration : INpgsqlDataSourceConfiguration
{
  public void Configure(NpgsqlDataSourceBuilder builder)
  {
    builder.MapEnum<AuditEntity>();
  }
}
