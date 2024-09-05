using Enms.Data.Entities.Base;
using Enms.Data.Entities.Complex;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Entities;

public class NetworkUserEntity : AuditableEntity
{
  public LegalPersonEntity LegalPerson { get; set; } = default!;

  public virtual ICollection<RepresentativeEntity> Representatives { get; set; } =
    default!;

  public virtual ICollection<LineEntity> Lines { get; set; } = default!;
}

public class NetworkUserEntityTypeConfiguration : EntityTypeConfiguration<
  NetworkUserEntity>
{
  public override void Configure(EntityTypeBuilder<NetworkUserEntity> builder)
  {
    builder.ToTable("network_users");

    builder.ComplexProperty(nameof(NetworkUserEntity.LegalPerson));
  }
}
