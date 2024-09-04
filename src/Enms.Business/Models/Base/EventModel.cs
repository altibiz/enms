using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Base;

public class EventModel : IEvent
{
  [Required]
  public required string Id { get; init; }

  [Required]
  public required string Title { get; init; }

  [Required]
  public required DateTimeOffset Timestamp { get; init; }

  [Required]
  public required LevelModel Level { get; init; }

  [Required]
  public required JsonDocument Content { get; init; }

  [Required]
  public required HashSet<CategoryModel> Categories { get; set; }

  public virtual IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    if (validationContext.ObjectInstance != this)
    {
      yield break;
    }

    if (
      validationContext.MemberName is null or nameof(Timestamp) &&
      Timestamp > DateTimeOffset.UtcNow
    )
    {
      yield return new ValidationResult(
        "Timestamp must be in the past",
        new[] { nameof(Timestamp) });
    }
  }
}
