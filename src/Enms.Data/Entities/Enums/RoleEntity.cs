using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

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
