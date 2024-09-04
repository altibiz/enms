using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class MeterNotificationEntity : ResolvableNotificationEntity
{
  public string MeterId { get; set; } = default!;

  public virtual MeterEntity Meter { get; set; } = default!;
}

public class MeterInactivityNotificationEntityConfiguration :
  IEntityTypeConfiguration<MeterNotificationEntity>
{
  public void Configure(EntityTypeBuilder<MeterNotificationEntity> builder)
  {
    builder
      .HasOne(nameof(MeterNotificationEntity.Meter))
      .WithOne(nameof(MeterEntity.Notifications))
      .HasForeignKey(nameof(MeterNotificationEntity.MeterId));
  }
}
