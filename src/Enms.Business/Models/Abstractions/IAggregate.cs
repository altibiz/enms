using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Abstractions;

public interface IAggregate : IValidatableObject, IReadonly
{
  public string LineId { get; }

  public DateTimeOffset Timestamp { get; }

  public IntervalModel Interval { get; }

  public long Count { get; }
}
