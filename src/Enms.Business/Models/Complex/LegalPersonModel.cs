using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Complex;

public class LegalPersonModel : IModel, IValidatableObject
{
  [Required]
  public required string Name { get; set; }

  [Required]
  public required string SocialSecurityNumber { get; set; }

  [Required]
  public required string Address { get; set; }

  [Required]
  public required string PostalCode { get; set; }

  [Required]
  public required string City { get; set; }

  [EmailAddress]
  [Required]
  public required string Email { get; set; }

  [Phone]
  [Required]
  public required string PhoneNumber { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (
      validationContext.MemberName == nameof(SocialSecurityNumber) &&
      !(SocialSecurityNumber.Length == 11 && SocialSecurityNumber.All(char.IsDigit)))
    {
      yield return new ValidationResult(
        "The social security number must be 11 digits long.",
        new[] { nameof(SocialSecurityNumber) });
    }

    if (validationContext.MemberName == nameof(PostalCode) && !(PostalCode.Length == 5 && PostalCode.All(char.IsDigit)))
    {
      yield return new ValidationResult(
        "The postal code must be 5 digits long.",
        new[] { nameof(PostalCode) });
    }
  }
}
