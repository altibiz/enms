using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;
using Enms.Business.Models.Complex;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models;

public class RepresentativeModel : AuditableModel
{
  [Required]
  public required RoleModel Role { get; set; }

  [Required]
  public required PhysicalPersonModel PhysicalPerson { get; set; } = default!;

  [Required]
  public required HashSet<TopicModel> Topics { get; set; } = default!;

  public string? NetworkUserId { get; set; } = default!;

  public override IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    foreach (var result in base.Validate(validationContext))
    {
      yield return result;
    }

    if (
      validationContext.MemberName is null or nameof(PhysicalPerson)
    )
    {
      foreach (var result in PhysicalPerson.Validate(validationContext))
      {
        yield return result;
      }
    }
  }
}
