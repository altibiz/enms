using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class EgaugeMeasurementEntity : MeasurementEntity<EgaugeMeterEntity>
{
#pragma warning disable CA1707
  public float Voltage_V { get; set; }

  public float Power_W { get; set; }
#pragma warning restore CA1707
}

public class
  EgaugeMeasurementEntityTypeConfiguration : EntityTypeConfiguration<
  EgaugeMeasurementEntity>
{
  public override void Configure(
    EntityTypeBuilder<EgaugeMeasurementEntity> builder)
  {
    builder.ToTable("egauge_measurements");

    builder
      .Property(nameof(EgaugeMeasurementEntity.Voltage_V))
      .HasColumnName("voltage_v");

    builder
      .Property(nameof(EgaugeMeasurementEntity.Power_W))
      .HasColumnName("power_w");
  }
}
