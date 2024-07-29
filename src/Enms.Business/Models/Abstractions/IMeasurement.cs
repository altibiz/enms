using System.ComponentModel.DataAnnotations;

namespace Enms.Business.Models.Abstractions;

public interface IMeasurement : IValidatableObject, IReadonly
{
  public string LineId { get; }

  public DateTimeOffset Timestamp { get; }
}
