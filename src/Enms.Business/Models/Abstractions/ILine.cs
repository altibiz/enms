using Enms.Business.Capabilities.Abstractions;

namespace Enms.Business.Models.Abstractions;

public interface ILine : IAuditable
{
  public string MeasurementValidatorId { get; }

  public string LineId { get; }

  public string MeterId { get; }

  public ILineCapabilities Capabilities { get; }

  public decimal ConnectionPower_W { get; }
}
