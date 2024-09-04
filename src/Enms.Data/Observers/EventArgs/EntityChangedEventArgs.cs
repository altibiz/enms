using Enms.Data.Entities.Abstractions;

namespace Enms.Data.Observers.EventArgs;

public class EntitiesChangedEventArgs : System.EventArgs
{
  public required IReadOnlyList<EntityChangedEntry> Entities { get; init; }
}

public sealed record EntityChangedEntry(
  EntityChangedState State,
  IEntity Entity
);

public enum EntityChangedState
{
  Added,
  Modified,
  Removed
}
