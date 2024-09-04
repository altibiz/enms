using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Complex;

public class PhysicalPersonModel : IModel, IValidatableObject
{
  [Required]
  public required string Name { get; set; } = default!;

  [EmailAddress]
  [Required]
  public required string Email { get; set; } = default!;

  [Phone]
  [Required]
  public required string PhoneNumber { get; set; } = default!;

  public IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    yield break;
  }
}
