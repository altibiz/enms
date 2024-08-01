using Enms.Data.Attributes;
using Enms.Data.Entities.Abstractions;
using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public abstract class AggregateEntity : IAggregateEntity
{
  public string MeterId { get; set; } = default!;
  public DateTimeOffset Timestamp { get; set; }

  public long Count { get; set; }

  public IntervalEntity Interval { get; set; }

  public string LineId { get; set; } = default!;
}

public class AggregateEntity<TLine, TMeter> : AggregateEntity
  where TLine : LineEntity
  where TMeter : MeterEntity
{
  public virtual TLine Line { get; set; } = default!;

  public virtual TMeter Meter { get; set; } = default!;
}

public class
  AggregateEntityTypeHierarchyConfiguration : EntityTypeHierarchyConfiguration<
  AggregateEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    if (entity == typeof(AggregateEntity))
    {
      return;
    }

    var builder = modelBuilder.Entity(entity);

    builder.HasKey(
      nameof(AggregateEntity.Interval),
      nameof(AggregateEntity.Timestamp),
      nameof(AggregateEntity.LineId),
      nameof(AggregateEntity.MeterId)
    );

    builder.HasTimescaleHypertable(
      nameof(AggregateEntity.Timestamp),
      nameof(AggregateEntity<LineEntity, MeterEntity>.MeterId),
      "number_partitions => 256"
    );

    builder
      .HasOne(nameof(AggregateEntity<LineEntity, MeterEntity>.Line))
      .WithMany(
        nameof(LineEntity<MeasurementEntity, AggregateEntity,
          MeasurementValidatorEntity, MeterEntity>.Aggregates))
      .HasForeignKey(
        nameof(AggregateEntity<LineEntity, MeterEntity>.LineId),
        nameof(AggregateEntity<LineEntity, MeterEntity>.MeterId));

    builder
      .HasOne(nameof(AggregateEntity<LineEntity, MeterEntity>.Meter))
      .WithMany()
      .HasForeignKey(
        nameof(AggregateEntity<LineEntity, MeterEntity>.MeterId));

    builder
      .Property<DateTimeOffset>(nameof(AggregateEntity.Timestamp))
      .HasConversion(
        x => x.ToUniversalTime(),
        x => x.ToUniversalTime()
      );
  }
}
