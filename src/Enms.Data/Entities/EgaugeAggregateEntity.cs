using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class EgaugeAggregateEntity : AggregateEntity<
  EgaugeLineEntity,
  EgaugeMeterEntity>
{
#pragma warning disable CA1707
  public float VoltageL1AnyT0Avg_V { get; set; }
  public float VoltageL2AnyT0Avg_V { get; set; }
  public float VoltageL3AnyT0Avg_V { get; set; }
  public float CurrentL1AnyT0Avg_A { get; set; }
  public float CurrentL2AnyT0Avg_A { get; set; }
  public float CurrentL3AnyT0Avg_A { get; set; }
  public float ActivePowerL1NetT0Avg_W { get; set; }
  public float ActivePowerL2NetT0Avg_W { get; set; }
  public float ActivePowerL3NetT0Avg_W { get; set; }
  public float ApparentPowerL1NetT0Avg_W { get; set; }
  public float ApparentPowerL2NetT0Avg_W { get; set; }
  public float ApparentPowerL3NetT0Avg_W { get; set; }
#pragma warning restore CA1707
}

public class
  EgaugeAggregateEntityTypeConfiguration : EntityTypeConfiguration<
  EgaugeAggregateEntity>
{
  public override void Configure(
    EntityTypeBuilder<EgaugeAggregateEntity> builder)
  {
    builder.ToTable("egauge_aggregates");

    builder
      .Property(nameof(EgaugeAggregateEntity.VoltageL1AnyT0Avg_V))
      .HasColumnName("voltage_l1_any_t0_avg_v");

    builder
      .Property(nameof(EgaugeAggregateEntity.VoltageL2AnyT0Avg_V))
      .HasColumnName("voltage_l2_any_t0_avg_v");

    builder
      .Property(nameof(EgaugeAggregateEntity.VoltageL3AnyT0Avg_V))
      .HasColumnName("voltage_l3_any_t0_avg_v");

    builder
      .Property(nameof(EgaugeAggregateEntity.CurrentL1AnyT0Avg_A))
      .HasColumnName("current_l1_any_t0_avg_a");

    builder
      .Property(nameof(EgaugeAggregateEntity.CurrentL2AnyT0Avg_A))
      .HasColumnName("current_l2_any_t0_avg_a");

    builder
      .Property(nameof(EgaugeAggregateEntity.CurrentL3AnyT0Avg_A))
      .HasColumnName("current_l3_any_t0_avg_a");

    builder
      .Property(nameof(EgaugeAggregateEntity.ActivePowerL1NetT0Avg_W))
      .HasColumnName("active_power_l1_net_t0_avg_w");

    builder
      .Property(nameof(EgaugeAggregateEntity.ActivePowerL2NetT0Avg_W))
      .HasColumnName("active_power_l2_net_t0_avg_w");

    builder
      .Property(nameof(EgaugeAggregateEntity.ActivePowerL3NetT0Avg_W))
      .HasColumnName("active_power_l3_net_t0_avg_w");

    builder
      .Property(nameof(EgaugeAggregateEntity.ApparentPowerL1NetT0Avg_W))
      .HasColumnName("apparent_power_l1_net_t0_avg_w");

    builder
      .Property(nameof(EgaugeAggregateEntity.ApparentPowerL2NetT0Avg_W))
      .HasColumnName("apparent_power_l2_net_t0_avg_w");

    builder
      .Property(nameof(EgaugeAggregateEntity.ApparentPowerL3NetT0Avg_W))
      .HasColumnName("apparent_power_l3_net_t0_avg_w");
  }
}
