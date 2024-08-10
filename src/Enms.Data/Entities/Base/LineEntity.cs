using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public class LineEntity : AuditableEntity
{
  protected string _lineId = default!;

  protected string _meterId = default!;

  public virtual string LineId
  {
    get { return _lineId; }
    init { _lineId = value; }
  }

  public virtual string MeterId
  {
    get { return _meterId; }
    init { _meterId = value; }
  }

  public override string Id
  {
    get { return $"{_lineId}{EnmsDataDbContext.KeyJoin}{_meterId}"; }
    init
    {
      var parts = value.Split(EnmsDataDbContext.KeyJoin);
      _lineId = parts[0];
      _meterId = parts[1];
    }
  }

  public float ConnectionPower_W { get; set; } = default!;

  public List<PhaseEntity> Phases { get; set; } = default!;
}

public class LineEntity<
  TMeasurement,
  TAggregate,
  TMeasurementValidator,
  TMeter
> : LineEntity
  where TMeasurement : MeasurementEntity
  where TAggregate : AggregateEntity
  where TMeasurementValidator : MeasurementValidatorEntity
  where TMeter : MeterEntity
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

  public virtual TMeter Meter { get; set; } = default!;
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
      .Property(nameof(LineEntity.ConnectionPower_W))
      .HasColumnName("connection_power_w");

    if (entity != typeof(LineEntity))
    {
      builder
        .HasMany(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity, MeterEntity>.Aggregates))
        .WithOne(nameof(AggregateEntity<LineEntity, MeterEntity>.Line));

      builder
        .HasMany(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity, MeterEntity>.Measurements))
        .WithOne(nameof(MeasurementEntity<LineEntity, MeterEntity>.Line));

      builder
        .HasOne(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity, MeterEntity>.MeasurementValidator))
        .WithMany(nameof(MeasurementValidatorEntity<LineEntity>.Lines))
        .HasForeignKey("_measurementValidatorId");

      builder.Ignore(
        nameof(LineEntity<MeasurementEntity, AggregateEntity,
          MeasurementValidatorEntity, MeterEntity>.MeasurementValidatorId));
      builder
        .Property("_measurementValidatorId")
        .HasColumnName("measurement_validator_id");

      builder
        .HasOne(
          nameof(LineEntity<MeasurementEntity, AggregateEntity,
            MeasurementValidatorEntity, MeterEntity>.Meter))
        .WithMany(nameof(MeterEntity.Lines))
        .HasForeignKey("_meterId");
    }
  }
}
