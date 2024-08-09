using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Base;

public abstract class IdentifiableModel : IIdentifiable
{
  public required string Id { get; set; }

  [Required]
  public required string Title { get; set; }

  public virtual IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    return Enumerable.Empty<ValidationResult>();
  }
}
