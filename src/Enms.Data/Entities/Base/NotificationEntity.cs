using Enms.Data.Entities.Abstractions;
using Enms.Data.Entities.Enums;
using Enms.Data.Entities.Joins;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public class NotificationEntity : IIdentifiableEntity, IReadonlyEntity
{
  protected readonly long? _eventId;

  protected readonly long _id;

  public DateTimeOffset Timestamp { get; set; }

  public string? EventId
  {
    get { return _eventId?.ToString(); }
    init
    {
      _eventId = value is { } notNullValue ? long.Parse(notNullValue) : null;
    }
  }

  public virtual EventEntity? Event { get; set; } = default!;

  public virtual ICollection<NotificationRecipientEntity> NotificationRecipients
  {
    get;
    set;
  } = default!;

  public virtual ICollection<RepresentativeEntity> Recipients { get; set; } =
    default!;

  public string Summary { get; set; } = default!;

  public string Content { get; set; } = default!;

  public List<TopicEntity> Topics { get; set; } = default!;

  public virtual string Id
  {
    get { return _id.ToString(); }
    init
    {
      _id = value is { } notNullValue ? long.Parse(notNullValue) : default;
    }
  }

  public string Title { get; set; } = default!;
}

public class
  AlertEntityTypeHierarchyConfiguration :
  EntityTypeHierarchyConfiguration<NotificationEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    var builder = modelBuilder.Entity(entity);

    if (entity == typeof(NotificationEntity))
    {
      builder.HasKey("_id");
    }

    builder
      .UseTphMappingStrategy()
      .ToTable("notifications")
      .HasDiscriminator<string>("kind");

    builder.Ignore(nameof(NotificationEntity.Id));
    builder
      .Property("_id")
      .HasColumnName("id")
      .HasColumnType("bigint")
      .UseIdentityAlwaysColumn();

    builder
      .Property<DateTimeOffset>(nameof(NotificationEntity.Timestamp))
      .HasConversion(
        x => x.ToUniversalTime(),
        x => x.ToUniversalTime()
      );

    builder
      .HasOne(nameof(NotificationEntity.Event))
      .WithMany(nameof(EventEntity.Notifications))
      .HasForeignKey("_eventId");

    builder.Ignore(nameof(NotificationEntity.EventId));
    builder
      .Property("_eventId")
      .HasColumnName("event_id")
      .HasColumnType("bigint");
  }
}
