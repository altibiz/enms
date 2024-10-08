using Enms.Data.Entities.Abstractions;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Entities.Base;

public abstract class AuditableEntity : IIdentifiableEntity
{
  protected readonly long _id;

  public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

  public string? CreatedById { get; set; }

  public virtual RepresentativeEntity? CreatedBy { get; set; }

  public DateTimeOffset? LastUpdatedOn { get; set; }

  public string? LastUpdatedById { get; set; }

  public virtual RepresentativeEntity? LastUpdatedBy { get; set; }

  public bool IsDeleted { get; set; }

  public DateTimeOffset? DeletedOn { get; set; }

  public string? DeletedById { get; set; }

  public virtual RepresentativeEntity? DeletedBy { get; set; }

  public virtual string Id
  {
    get { return _id.ToString(); }
    init { _id = value is { } nonNull ? long.Parse(nonNull) : default!; }
  }

  public string Title { get; set; } = default!;
}

public class
  AuditableEntityConfiguration : EntityTypeHierarchyConfiguration<
  AuditableEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    if (entity == typeof(AuditableEntity))
    {
      return;
    }

    var builder = modelBuilder.Entity(entity);

    if (entity.BaseType == typeof(AuditableEntity))
    {
      if (entity == typeof(LineEntity))
      {
        builder.HasKey("_lineId", "_meterId");
      }
      else if (
        entity == typeof(RepresentativeEntity)
        || entity == typeof(MeterEntity))
      {
        builder.HasKey("_stringId");
      }
      else
      {
        builder.HasKey("_id");
      }
    }

    if (entity.IsAssignableTo(typeof(LineEntity)))
    {
      builder.Ignore("_id");
      builder.Ignore(nameof(AuditableEntity.Id));
      builder.Ignore(nameof(LineEntity.LineId));
      builder
        .Property("_lineId")
        .HasColumnName("line_id")
        .HasColumnType("text")
        .ValueGeneratedNever();
      builder.Ignore(nameof(LineEntity.MeterId));
      builder
        .Property("_meterId")
        .HasColumnName("meter_id")
        .HasColumnType("text")
        .ValueGeneratedNever();
    }
    else if (entity == typeof(RepresentativeEntity) ||
      entity.IsAssignableTo(typeof(MeterEntity)))
    {
      builder.Ignore("_id");
      builder.Ignore(nameof(AuditableEntity.Id));
      builder
        .Property("_stringId")
        .HasColumnName("id")
        .HasColumnType("text")
        .ValueGeneratedNever();
    }
    else
    {
      builder.Ignore(nameof(AuditableEntity.Id));
      builder
        .Property("_id")
        .HasColumnName("id")
        .HasColumnType("bigint")
        .UseIdentityAlwaysColumn();
    }

    builder
      .HasOne(nameof(AuditableEntity.CreatedBy))
      .WithMany()
      .HasForeignKey(nameof(AuditableEntity.CreatedById));

    builder
      .HasOne(nameof(AuditableEntity.LastUpdatedBy))
      .WithMany()
      .HasForeignKey(nameof(AuditableEntity.LastUpdatedById));

    builder
      .HasOne(nameof(AuditableEntity.DeletedBy))
      .WithMany()
      .HasForeignKey(nameof(AuditableEntity.DeletedById));
  }
}
