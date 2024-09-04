using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

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
