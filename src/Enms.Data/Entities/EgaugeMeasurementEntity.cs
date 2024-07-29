using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class EgaugeMeasurementEntity : MeasurementEntity<EgaugeLineEntity>
{
#pragma warning disable CA1707
  public float VoltageL1AnyT0_V { get; set; }
  public float VoltageL2AnyT0_V { get; set; }
  public float VoltageL3AnyT0_V { get; set; }
  public float CurrentL1AnyT0_A { get; set; }
  public float CurrentL2AnyT0_A { get; set; }
  public float CurrentL3AnyT0_A { get; set; }
  public float ActivePowerL1NetT0_W { get; set; }
  public float ActivePowerL2NetT0_W { get; set; }
  public float ActivePowerL3NetT0_W { get; set; }
  public float ApparentPowerL1NetT0_W { get; set; }
  public float ApparentPowerL2NetT0_W { get; set; }
  public float ApparentPowerL3NetT0_W { get; set; }
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
      .Property(nameof(EgaugeMeasurementEntity.VoltageL1AnyT0_V))
      .HasColumnName("voltage_l1_any_t0_v");

    builder
      .Property(nameof(EgaugeMeasurementEntity.VoltageL2AnyT0_V))
      .HasColumnName("voltage_l2_any_t0_v");

    builder
      .Property(nameof(EgaugeMeasurementEntity.VoltageL3AnyT0_V))
      .HasColumnName("voltage_l3_any_t0_v");

    builder
      .Property(nameof(EgaugeMeasurementEntity.CurrentL1AnyT0_A))
      .HasColumnName("current_l1_any_t0_a");

    builder
      .Property(nameof(EgaugeMeasurementEntity.CurrentL2AnyT0_A))
      .HasColumnName("current_l2_any_t0_a");

    builder
      .Property(nameof(EgaugeMeasurementEntity.CurrentL3AnyT0_A))
      .HasColumnName("current_l3_any_t0_a");

    builder
      .Property(nameof(EgaugeMeasurementEntity.ActivePowerL1NetT0_W))
      .HasColumnName("active_power_l1_net_t0_w");

    builder
      .Property(nameof(EgaugeMeasurementEntity.ActivePowerL2NetT0_W))
      .HasColumnName("active_power_l2_net_t0_w");

    builder
      .Property(nameof(EgaugeMeasurementEntity.ActivePowerL3NetT0_W))
      .HasColumnName("active_power_l3_net_t0_w");

    builder
      .Property(nameof(EgaugeMeasurementEntity.ApparentPowerL1NetT0_W))
      .HasColumnName("apparent_power_l1_net_t0_w");

    builder
      .Property(nameof(EgaugeMeasurementEntity.ApparentPowerL2NetT0_W))
      .HasColumnName("apparent_power_l2_net_t0_w");

    builder
      .Property(nameof(EgaugeMeasurementEntity.ApparentPowerL3NetT0_W))
      .HasColumnName("apparent_power_l3_net_t0_w");
  }
}
