namespace Enms.Jobs.Observers.EventArgs;

public class MeterInactivityEventArgs : System.EventArgs
{
  public required string Id { get; init; }
}
