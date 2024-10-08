using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

// TODO: extension method for querying auditable entities from these events
// TODO: check if auditable entity type or table is modified that
// migration also modified events

namespace Enms.Data.Entities.Base;

public class AuditEventEntity : EventEntity
{
  public string AuditableEntityId { get; set; } = default!;

  public string AuditableEntityType { get; set; } = default!;

  public string AuditableEntityTable { get; set; } = default!;

  public AuditEntity Audit { get; set; } = default!;
}

public class AuditEventEntityTypeHierarchyConfiguration :
  EntityTypeHierarchyConfiguration<AuditEventEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    var builder = modelBuilder.Entity(entity);

    builder.HasIndex(
      new[]
      {
        nameof(AuditEventEntity.AuditableEntityType),
        nameof(AuditEventEntity.AuditableEntityId)
      },
      "ix_events_auditable_entity_type_auditable_entity_id"
    );

    builder.HasIndex(
      new[]
      {
        nameof(AuditEventEntity.AuditableEntityTable),
        nameof(AuditEventEntity.AuditableEntityId)
      },
      "ix_events_auditable_entity_table_auditable_entity_id"
    );
  }
}
