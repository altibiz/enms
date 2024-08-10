using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Time;

namespace Enms.Business.Models.Base;

public abstract class AggregateModel : IAggregate
{
  private IntervalModel _interval = IntervalModel.QuarterHour;

  // NOTE: just so it doesn't break if interval is set before timestamp
  private DateTimeOffset _timestamp =
    DateTimeOffset.Parse("2000-01-01T00:00:00Z", CultureInfo.InvariantCulture);

  [Required]
  public required string MeterId { get; set; }

  [Required]
  public required string LineId { get; set; }

  [Required]
  public required DateTimeOffset Timestamp
  {
    get { return _timestamp.ToUniversalTime(); }
    set
    {
      _ = Interval switch
      {
#pragma warning disable S1121
        IntervalModel.QuarterHour => _timestamp = value.GetStartOfQuarterHour(),
        IntervalModel.Day => _timestamp = value.GetStartOfDay(),
        IntervalModel.Month => _timestamp = value.GetStartOfMonth(),
#pragma warning restore S1121
        _ => throw new InvalidOperationException(
          $"Unsupported interval {Interval}"
        )
      };
    }
  }

  [Required]
  public required IntervalModel Interval
  {
    get { return _interval; }
    set
    {
      _interval = value;
      Timestamp = _timestamp;
    }
  }

  [Required]
  public required long Count { get; init; } = 0;

  public virtual IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    yield break;
  }
}
