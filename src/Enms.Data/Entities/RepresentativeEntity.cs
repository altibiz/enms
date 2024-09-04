using Enms.Data.Entities.Base;
using Enms.Data.Entities.Complex;
using Enms.Data.Entities.Enums;
using Enms.Data.Entities.Joins;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class RepresentativeEntity : AuditableEntity
{
  protected readonly string _stringId = default!;

  public override string Id
  {
    get { return _stringId; }
    init { _stringId = value; }
  }

  public RoleEntity Role { get; set; }

  public virtual ICollection<RepresentativeEventEntity> Events { get; set; } =
    default!;

  public virtual ICollection<RepresentativeAuditEventEntity> AuditEvents
  {
    get;
    set;
  } =
    default!;

  public virtual ICollection<NotificationRecipientEntity> NotificationRecipients
  {
    get;
    set;
  } = default!;

  public virtual ICollection<NotificationEntity> Notifications { get; set; } =
    default!;

  public virtual ICollection<ResolvableNotificationEntity> ResolvedNotifications
  {
    get;
    set;
  } = default!;

  public PhysicalPersonEntity PhysicalPerson { get; set; } = default!;

  public List<TopicEntity> Topics { get; set; } = default!;
}

public class
  RepresentativeEntityTypeConfiguration : EntityTypeConfiguration<
  RepresentativeEntity>
{
  public override void Configure(
    EntityTypeBuilder<RepresentativeEntity> builder)
  {
    builder.ComplexProperty(nameof(RepresentativeEntity.PhysicalPerson));
  }
}
