using Enms.Business.Models.Abstractions;

namespace Enms.Business.Pushing.EventArgs;

public class MeasurementPushEventArgs(IReadOnlyList<IMeasurement> measurements) : System.EventArgs
{
  public IReadOnlyList<IMeasurement> Measurements { get; } = measurements;
}
