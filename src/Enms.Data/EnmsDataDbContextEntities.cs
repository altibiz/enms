using Enms.Data.Entities;
using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data;

public partial class EnmsDataDbContext : DbContext
{
  public DbSet<RepresentativeEntity> Representatives { get; set; } = default!;

  public DbSet<MeterEntity> Meters { get; set; } = default!;

  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public DbSet<EgaugeAggregateEntity> EgaugeAggregates { get; set; } = default!;

  public DbSet<EventEntity> Events { get; set; } = default!;
}
