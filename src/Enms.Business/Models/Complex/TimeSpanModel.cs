using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Complex;

public class TimeSpanModel : IModel, IValidatableObject
{
  public DurationModel Duration { get; set; }

  public uint Multiplier { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    yield break;
  }
}

public static class TimeSpanModelExtensions
{
  public static TimeSpan ToTimeSpan(this TimeSpanModel model)
  {
    return model.Duration.ToTimeSpan() * model.Multiplier;
  }
}
