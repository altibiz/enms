using Enms.Data.Entities.Base;
using Enms.Data.Entities.Complex;
using Enms.Data.Entities.Enums;
using Enms.Data.Entities.Joins;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class RepresentativeEntity : AuditableEntity
{
  protected readonly string _stringId = default!;

  protected readonly long? _networkUserId = default!;

  public override string Id
  {
    get { return _stringId; }
    init { _stringId = value; }
  }

  public virtual string? NetworkUserId
  {
    get { return _networkUserId?.ToString(); }
    init { _networkUserId = value is { } nonNull ? long.Parse(nonNull) : null; }
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

  public virtual NetworkUserEntity? NetworkUser { get; set; }
}

public class
  RepresentativeEntityTypeConfiguration : EntityTypeConfiguration<
  RepresentativeEntity>
{
  public override void Configure(
    EntityTypeBuilder<RepresentativeEntity> builder)
  {
    builder.ComplexProperty(nameof(RepresentativeEntity.PhysicalPerson));

    builder
      .HasOne(nameof(RepresentativeEntity.NetworkUser))
      .WithMany(nameof(NetworkUserEntity.Representatives))
      .HasForeignKey("_networkUserId");
    builder.Ignore(nameof(RepresentativeEntity.NetworkUserId));
    builder
      .Property("_networkUserId")
      .HasColumnName("network_user_id");
  }
}
