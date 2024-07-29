namespace Enms.Data.Entities.Abstractions;

public interface IMeasurementEntity : IReadonlyEntity
{
  public DateTimeOffset Timestamp { get; }

  public string LineId { get; }
}
