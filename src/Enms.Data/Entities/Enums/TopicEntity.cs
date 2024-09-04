using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Enums;

public enum TopicEntity
{
  All,
  Meter,
  MeterInactivity
}

public class TopicEntityModelConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresEnum<TopicEntity>();
  }
}
