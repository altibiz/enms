using Enms.Data.Attributes;
using Enms.Data.Entities.Abstractions;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public abstract class MeasurementEntity : IMeasurementEntity
{
  public DateTimeOffset Timestamp { get; set; }

  public string LineId { get; set; } = default!;
}

public class MeasurementEntity<T> : MeasurementEntity
  where T : LineEntity
{
  public virtual T Line { get; set; } = default!;
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
      nameof(MeasurementEntity.LineId)
    );

    builder.HasTimescaleHypertable(
      nameof(MeasurementEntity.Timestamp),
      nameof(MeasurementEntity<LineEntity>.LineId),
      "number_partitions => 2"
    );

    builder
      .HasOne(nameof(MeasurementEntity<LineEntity>.Line))
      .WithMany()
      .HasForeignKey(nameof(MeasurementEntity<LineEntity>.LineId));

    builder
      .Property<DateTimeOffset>(nameof(MeasurementEntity.Timestamp))
      .HasConversion(
        x => x.ToUniversalTime(),
        x => x.ToUniversalTime()
      );
  }
}
