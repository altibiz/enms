using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public class ResolvableNotificationEntity : NotificationEntity
{
  public string? ResolvedById { get; set; } = default!;

  public virtual RepresentativeEntity? ResolvedBy { get; set; } = default!;

  public DateTimeOffset? ResolvedOn { get; set; } = default!;
}

public class ResolvableNotificationEntityModelConfiguration :
  EntityTypeHierarchyConfiguration<ResolvableNotificationEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    var builder = modelBuilder.Entity(entity);

    builder
      .HasOne(nameof(ResolvableNotificationEntity.ResolvedBy))
      .WithMany(nameof(RepresentativeEntity.ResolvedNotifications))
      .HasForeignKey(nameof(ResolvableNotificationEntity.ResolvedById));
  }
}
