using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

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
