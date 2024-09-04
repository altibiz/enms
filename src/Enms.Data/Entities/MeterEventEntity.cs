using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class MeterEventEntity : EventEntity
{
  public string MeterId { get; set; } = default!;

  public virtual MeterEntity Meter { get; set; } = default!;
}

public class
  MeterEventEntityTypeConfiguration : EntityTypeConfiguration<
  MeterEventEntity>
{
  public override void Configure(
    EntityTypeBuilder<MeterEventEntity> builder)
  {
    builder
      .HasOne(nameof(MeterEventEntity.Meter))
      .WithMany(nameof(MeterEntity.Events))
      .HasForeignKey(nameof(MeterEventEntity.MeterId));
  }
}
