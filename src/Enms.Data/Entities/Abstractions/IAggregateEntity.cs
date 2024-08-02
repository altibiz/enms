using Enms.Data.Entities.Enums;

namespace Enms.Data.Entities.Abstractions;

public interface IAggregateEntity : IReadonlyEntity
{
  public DateTimeOffset Timestamp { get; }

  public long Count { get; }

  public IntervalEntity Interval { get; }

  public string MeterId { get; }

  public string LineId { get; }
}
