using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

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
