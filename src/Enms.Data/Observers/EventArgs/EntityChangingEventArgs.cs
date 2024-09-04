using Enms.Data.Entities.Abstractions;

namespace Enms.Data.Observers.EventArgs;

public class EntitiesChangingEventArgs : System.EventArgs
{
  public required IReadOnlyList<EntityChangingRecord> Entities { get; init; }
}

public sealed record EntityChangingRecord(
  EntityChangingState State,
  IEntity Entity
);

public enum EntityChangingState
{
  Adding,
  Modifying,
  Removing
}
