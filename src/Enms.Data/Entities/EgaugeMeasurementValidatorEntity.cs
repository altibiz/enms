using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// TODO: more clear naming for properties

namespace Enms.Data.Entities;

public class
  EgaugeMeasurementValidatorEntity : MeasurementValidatorEntity<
  EgaugeMeterEntity>
{
#pragma warning disable CA1707
#pragma warning restore CA1707
}

public class
  EgaugeMeasurementValidatorEntityTypeConfiguration : EntityTypeConfiguration<
  EgaugeMeasurementValidatorEntity>
{
  public override void Configure(
    EntityTypeBuilder<EgaugeMeasurementValidatorEntity> builder)
  {
  }
}
