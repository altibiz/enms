using Enms.Data.Attributes;
using Enms.Data.Entities.Abstractions;
using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public abstract class AggregateEntity : IAggregateEntity
{
  public DateTimeOffset Timestamp { get; set; }

  public long Count { get; set; }

  public IntervalEntity Interval { get; set; }

  public string MeterId { get; set; } = default!;
}

public class AggregateEntity<T> : AggregateEntity
  where T : MeterEntity
{
  public virtual T Meter { get; set; } = default!;
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
      nameof(AggregateEntity.Timestamp),
      nameof(AggregateEntity.Interval),
      nameof(AggregateEntity.MeterId)
    );

    builder.HasTimescaleHypertable(
      nameof(AggregateEntity.Timestamp),
      nameof(AggregateEntity<MeterEntity>.MeterId),
      "number_partitions => 2"
    );

    builder
      .HasOne(nameof(AggregateEntity<MeterEntity>.Meter))
      .WithMany()
      .HasForeignKey(nameof(AggregateEntity<MeterEntity>.MeterId));

    builder
      .Property<DateTimeOffset>(nameof(AggregateEntity.Timestamp))
      .HasConversion(
        x => x.ToUniversalTime(),
        x => x.ToUniversalTime()
      );
  }
}
