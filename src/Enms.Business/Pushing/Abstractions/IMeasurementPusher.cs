using Enms.Business.Models.Abstractions;

namespace Enms.Business.Pushing.Abstractions;

public interface IMeasurementPusher
{
  public Task Push(IEnumerable<IMeasurement> measurements);
}
