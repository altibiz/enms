using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public class LineEntity : AuditableEntity
{
  protected readonly string _stringId = default!;

  public override string Id
  {
    get { return _stringId; }
    init { _stringId = value; }
  }

  public string MessengerId { get; set; } = default!;

  public float ConnectionPower_W { get; set; } = default!;

  public List<PhaseEntity> Phases { get; set; } = default!;

  public string? MeterId { get; set; } = default!;

  public virtual MeterEntity? Meter { get; set; } = default!;
}

public class LineEntity<
  TMeasurement,
  TAggregate,
  TMeasurementValidator
> : LineEntity
  where TMeasurement : MeasurementEntity
  where TAggregate : AggregateEntity
  where TMeasurementValidator : MeasurementValidatorEntity
{
  private readonly long _measurementValidatorId;

  public virtual ICollection<TMeasurement> Measurements { get; set; } =
    default!;

  public virtual ICollection<TAggregate> Aggregates { get; set; } = default!;

  public virtual string MeasurementValidatorId
  {
    get { return _measurementValidatorId.ToString(); }
    init { _measurementValidatorId = long.Parse(value); }
  }

  public virtual TMeasurementValidator MeasurementValidator { get; set; } =
    default!;
}

public class
  LineInheritedEntityTypeConfiguration :
  EntityTypeHierarchyConfiguration<LineEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    var builder = modelBuilder.Entity(entity);

    builder
      .UseTphMappingStrategy()
      .ToTable("lines")
      .HasDiscriminator<string>("kind");

    builder
      .HasOne(nameof(LineEntity.Meter))
      .WithMany(nameof(MeterEntity.Lines))
      .HasForeignKey(nameof(LineEntity.MeterId));

    builder
      .Property(nameof(LineEntity.ConnectionPower_W))
      .HasColumnName("connection_power_w");

    if (entity != typeof(LineEntity))
    {
      builder
        .HasMany(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity>.Measurements))
        .WithOne(nameof(MeasurementEntity<LineEntity>.Line));

      builder

        .HasMany(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity>.Aggregates))
        .WithOne(nameof(AggregateEntity<LineEntity>.Line));

      builder
        .HasOne(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity>.MeasurementValidator))
        .WithOne(nameof(MeasurementValidatorEntity<LineEntity>.Line))
        .HasForeignKey(entity.Name, "_measurementValidatorId");

      builder.Ignore(
        nameof(LineEntity<MeasurementEntity, AggregateEntity,
          MeasurementValidatorEntity>.MeasurementValidatorId));
      builder
        .Property("_measurementValidatorId")
        .HasColumnName("measurement_validator_id");
    }
  }
}
