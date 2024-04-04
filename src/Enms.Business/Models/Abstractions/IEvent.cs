using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Abstractions;

public interface IEvent : IValidatableObject, IIdentifiable, IReadonly
{
  public DateTimeOffset Timestamp { get; }

  public LevelModel Level { get; }

  public string Description { get; }
}
