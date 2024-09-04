using Enms.Data.Entities.Abstractions;

namespace Enms.Data.Observers.EventArgs;

public class EntityAddedEventArgs : System.EventArgs
{
  public required IEntity Entity { get; init; }
}

public class EntityAddingEventArgs : System.EventArgs
{
  public required IEntity Entity { get; init; }
}

public class EntityRemovedEventArgs : System.EventArgs
{
  public required IEntity Entity { get; init; }
}

public class EntityRemovingEventArgs : System.EventArgs
{
  public required IEntity Entity { get; init; }
}

public class EntityModifiedEventArgs : System.EventArgs
{
  public required IEntity Entity { get; init; }
}

public class EntityModifyingEventArgs : System.EventArgs
{
  public required IEntity Entity { get; init; }
}
