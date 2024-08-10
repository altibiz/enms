using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities;

public abstract class MeasurementValidatorEntity : AuditableEntity
{
}

public class MeasurementValidatorEntity<TLine> : MeasurementValidatorEntity
  where TLine : LineEntity
{
  public virtual ICollection<TLine> Lines { get; set; } = default!;
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

    if (entity != typeof(MeasurementValidatorEntity))
    {
      builder
        .HasMany(nameof(MeasurementValidatorEntity<LineEntity>.Lines))
        .WithOne(
          nameof(
            LineEntity<MeasurementEntity, AggregateEntity,
              MeasurementValidatorEntity, MeterEntity>.MeasurementValidator));
    }
  }
}
