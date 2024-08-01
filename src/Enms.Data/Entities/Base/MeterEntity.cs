using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

// TODO: settings for the meter

namespace Enms.Data.Entities.Base;

public class MeterEntity : AuditableEntity
{
  protected readonly string _stringId = default!;

  public override string Id
  {
    get { return _stringId; }
    init { _stringId = value; }
  }

  public virtual ICollection<LineEntity> Lines { get; set; } = default!;
}

public class
  MessengerEntityTypeConfiguration : EntityTypeHierarchyConfiguration<
  MeterEntity>
{
  public override void Configure(ModelBuilder modelBuilder, Type entity)
  {
    var builder = modelBuilder.Entity(entity);

    builder
      .UseTphMappingStrategy()
      .ToTable("meters")
      .HasDiscriminator<string>("kind");
  }
}
