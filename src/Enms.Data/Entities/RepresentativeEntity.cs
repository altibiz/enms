using Enms.Data.Entities.Base;
using Enms.Data.Entities.Enums;

namespace Enms.Data.Entities;

public class RepresentativeEntity : AuditableEntity
{
  protected readonly string _stringId = default!;

  public override string Id
  {
    get { return _stringId; }
    init { _stringId = value; }
  }

  public RoleEntity Role { get; set; }

  public virtual ICollection<RepresentativeEventEntity> Events { get; set; } =
    default!;

  public virtual ICollection<RepresentativeAuditEventEntity> AuditEvents
  {
    get;
    set;
  } =
    default!;

  public string Name { get; set; } = default!;

  public string SocialSecurityNumber { get; set; } = default!;

  public string Address { get; set; } = default!;

  public string PostalCode { get; set; } = default!;

  public string City { get; set; } = default!;

  public string Email { get; set; } = default!;

  public string PhoneNumber { get; set; } = default!;
}
