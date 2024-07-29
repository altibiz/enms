using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Base;

public abstract class AggregateModel : IAggregate
{
  [Required]
  public required string LineId { get; set; }

  [Required]
  public required DateTimeOffset Timestamp { get; init; } =
    DateTimeOffset.UtcNow;

  [Required]
  public required IntervalModel Interval { get; init; }

  [Required]
  public required long Count { get; init; } = 0;

  public virtual IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    yield break;
  }
}
