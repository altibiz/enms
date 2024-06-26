using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class RepresentativeEventEntity : EventEntity
{
  public string RepresentativeId { get; set; } = default!;

  public virtual RepresentativeEntity Representative { get; set; } = default!;
}

public class
  RepresentativeEventEntityConfiguration : EntityTypeConfiguration<
  RepresentativeEventEntity>
{
  public override void Configure(
    EntityTypeBuilder<RepresentativeEventEntity> builder)
  {
    builder
      .HasOne(nameof(RepresentativeEventEntity.Representative))
      .WithMany(nameof(RepresentativeEntity.Events))
      .HasForeignKey(nameof(RepresentativeEventEntity.RepresentativeId));

    builder
      .Property(nameof(RepresentativeAuditEventEntity.RepresentativeId))
      .HasColumnName("representative_id");
  }
}
