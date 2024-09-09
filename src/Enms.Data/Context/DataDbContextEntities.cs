using Enms.Data.Entities;
using Enms.Data.Entities.Base;
using Enms.Data.Entities.Joins;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Context;

public partial class DataDbContext : DbContext
{
  public DbSet<RepresentativeEntity> Representatives { get; set; } = default!;

  public DbSet<MeterEntity> Meters { get; set; } = default!;

  public DbSet<LineEntity> Lines { get; set; } = default!;

  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public DbSet<EgaugeAggregateEntity> EgaugeAggregates { get; set; } = default!;

  public DbSet<EventEntity> Events { get; set; } = default!;

  public DbSet<NotificationEntity> Notifications { get; set; } = default!;

  public DbSet<NotificationRecipientEntity> NotificationRecipients { get; set; } =
    default!;

  public DbSet<NetworkUserEntity> NetworkUsers { get; set; } = default!;
}
