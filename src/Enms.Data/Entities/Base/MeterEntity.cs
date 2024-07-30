using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
  MessengerEntityTypeConfiguration : EntityTypeConfiguration<MeterEntity>
{
  public override void Configure(EntityTypeBuilder<MeterEntity> builder)
  {
    builder
      .HasMany(nameof(MeterEntity.Lines))
      .WithOne(nameof(LineEntity.Meter));
  }
}
