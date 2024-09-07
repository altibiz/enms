using Enms.Data.Entities.Enums;

namespace Enms.Data.Entities.Complex;

public class PeriodEntity
{
  public DurationEntity Duration { get; set; }

  public uint Multiplier { get; set; }
}
