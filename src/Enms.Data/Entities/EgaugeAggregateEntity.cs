using Enms.Data.Entities.Base;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class EgaugeAggregateEntity : AggregateEntity<EgaugeMeterEntity>
{
#pragma warning disable CA1707
#pragma warning restore CA1707
}

public class
  EgaugeAggregateEntityTypeConfiguration : EntityTypeConfiguration<
  EgaugeAggregateEntity>
{
  public override void Configure(
    EntityTypeBuilder<EgaugeAggregateEntity> builder)
  {
    builder.ToTable("egauge_aggregates");
  }
}
