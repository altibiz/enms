using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities;

public abstract class MeasurementValidatorEntity : AuditableEntity
{
}

public class MeasurementValidatorEntity<TMeter> : MeasurementValidatorEntity
  where TMeter : MeterEntity
{
  public virtual TMeter Meter { get; set; } = default!;
}

public class MeasurementValidatorEntityTypeHierarchyConfiguration :
  EntityTypeHierarchyConfiguration<MeasurementValidatorEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    var builder = modelBuilder.Entity(entity);

    builder
      .UseTphMappingStrategy()
      .ToTable("measurement_validators")
      .HasDiscriminator<string>("kind");
  }
}
