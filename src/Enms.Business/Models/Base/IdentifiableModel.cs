using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Base;

public abstract class IdentifiableModel : IIdentifiable
{
  [Required]
  public required string Id { get; init; }

  [Required]
  public required string Title { get; set; }

  public virtual IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    return Enumerable.Empty<ValidationResult>();
  }
}
