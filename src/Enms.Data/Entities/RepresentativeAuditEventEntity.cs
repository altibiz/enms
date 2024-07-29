using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class RepresentativeAuditEventEntity : AuditEventEntity
{
  public string RepresentativeId { get; set; } = default!;

  public virtual RepresentativeEntity Representative { get; set; } = default!;
}

public class
  RepresentativeAuditEventEntityConfiguration : EntityTypeConfiguration<
  RepresentativeAuditEventEntity>
{
  public override void Configure(
    EntityTypeBuilder<RepresentativeAuditEventEntity> builder)
  {
    builder
      .HasOne(nameof(RepresentativeAuditEventEntity.Representative))
      .WithMany(nameof(RepresentativeEntity.AuditEvents))
      .HasForeignKey(nameof(RepresentativeAuditEventEntity.RepresentativeId));

    builder
      .Property(nameof(RepresentativeAuditEventEntity.RepresentativeId))
      .HasColumnName("representative_id");
  }
}
