using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Joins;

public class NotificationRecipientEntity
{
  private long _notificationId;

  public string NotificationId
  {
    get { return _notificationId.ToString(); }
    set
    {
      _notificationId = value is { } notNullValue
        ? long.Parse(notNullValue)
        : default;
    }
  }

  public string RecipientId { get; set; } = default!;

  public virtual NotificationEntity Notification { get; set; } = default!;

  public virtual RepresentativeEntity Recipient { get; set; } = default!;

  public DateTimeOffset? SeenOn { get; set; } = default!;
}

public class NotificationRecipientEntityModelConfiguration : IModelConfiguration
{
  public void Configure(ModelBuilder modelBuilder)
  {
    var entity = modelBuilder.Entity<NotificationEntity>();

    entity
      .HasMany(nameof(NotificationEntity.Recipients))
      .WithMany(nameof(RepresentativeEntity.Notifications))
      .UsingEntity(
        typeof(NotificationRecipientEntity),
        configureLeft: l => l
          .HasOne(nameof(NotificationRecipientEntity.Notification))
          .WithMany(nameof(NotificationEntity.NotificationRecipients))
          .HasForeignKey("_notificationId"),
        configureRight: r => r
          .HasOne(nameof(NotificationRecipientEntity.Recipient))
          .WithMany(nameof(RepresentativeEntity.NotificationRecipients))
          .HasForeignKey(nameof(NotificationRecipientEntity.RecipientId)),
        configureJoinEntityType: entity =>
        {
          entity.Ignore(nameof(NotificationRecipientEntity.NotificationId));
          entity
            .Property("_notificationId")
            .HasColumnName("notification_id");
        }
      );
  }
}
