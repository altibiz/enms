using Enms.Business.Models.Abstractions;

namespace Enms.Business.Observers.EventArgs;

public class MeasurementUpsertEventArgs : System.EventArgs
{
  public IReadOnlyList<IMeasurement> Measurements { get; set; } = default!;

  public IReadOnlyList<IAggregate> Aggregates { get; set; } = default!;
}
