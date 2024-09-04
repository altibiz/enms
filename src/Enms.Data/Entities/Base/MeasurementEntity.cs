using Enms.Data.Entities.Abstractions;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public abstract class MeasurementEntity : IMeasurementEntity
{
  public string MeterId { get; set; } = default!;
  public DateTimeOffset Timestamp { get; set; }

  public string LineId { get; set; } = default!;
}

public class MeasurementEntity<TLine, TMeter> : MeasurementEntity
  where TLine : LineEntity
  where TMeter : MeterEntity
{
  public virtual TLine Line { get; set; } = default!;

  public virtual TMeter Meter { get; set; } = default!;
}

public class
  MeasurementEntityTypeHierarchyConfiguration : EntityTypeHierarchyConfiguration
<
  MeasurementEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    if (entity == typeof(MeasurementEntity))
    {
      return;
    }

    var builder = modelBuilder.Entity(entity);

    builder.HasKey(
      nameof(MeasurementEntity.Timestamp),
      nameof(MeasurementEntity.LineId),
      nameof(MeasurementEntity.MeterId)
    );

    builder.HasTimescaleHypertable(
      nameof(MeasurementEntity.Timestamp),
      nameof(MeasurementEntity<LineEntity, MeterEntity>.MeterId),
      "number_partitions => 256"
    );

    builder
      .HasOne(nameof(MeasurementEntity<LineEntity, MeterEntity>.Line))
      .WithMany(
        nameof(LineEntity<MeasurementEntity, AggregateEntity,
          MeasurementValidatorEntity, MeterEntity>.Measurements))
      .HasForeignKey(
        nameof(MeasurementEntity<LineEntity, MeterEntity>.LineId),
        nameof(MeasurementEntity<LineEntity, MeterEntity>.MeterId));

    builder
      .HasOne(nameof(MeasurementEntity<LineEntity, MeterEntity>.Meter))
      .WithMany()
      .HasForeignKey(
        nameof(MeasurementEntity<LineEntity, MeterEntity>.MeterId));

    builder
      .Property<DateTimeOffset>(nameof(MeasurementEntity.Timestamp))
      .HasConversion(
        x => x.ToUniversalTime(),
        x => x.ToUniversalTime()
      );
  }
}
