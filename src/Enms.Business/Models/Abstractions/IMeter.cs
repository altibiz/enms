using Enms.Business.Capabilities.Abstractions;

namespace Enms.Business.Models.Abstractions;

public interface IMeter : IAuditable
{
  public string MeasurementValidatorId { get; }

  public ICapabilities Capabilities { get; }
}
