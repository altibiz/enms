using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class EgaugeAggregateModel : AggregateModel
{
  [Required]
  public required decimal VoltageL1AnyT0Avg_V { get; set; }

  [Required]
  public required decimal VoltageL2AnyT0Avg_V { get; set; }

  [Required]
  public required decimal VoltageL3AnyT0Avg_V { get; set; }

  [Required]
  public required decimal CurrentL1AnyT0Avg_A { get; set; }

  [Required]
  public required decimal CurrentL2AnyT0Avg_A { get; set; }

  [Required]
  public required decimal CurrentL3AnyT0Avg_A { get; set; }

  [Required]
  public required decimal ActivePowerL1NetT0Avg_W { get; set; }

  [Required]
  public required decimal ActivePowerL2NetT0Avg_W { get; set; }

  [Required]
  public required decimal ActivePowerL3NetT0Avg_W { get; set; }

  [Required]
  public required decimal ApparentPowerL1NetT0Avg_W { get; set; }

  [Required]
  public required decimal ApparentPowerL2NetT0Avg_W { get; set; }

  [Required]
  public required decimal ApparentPowerL3NetT0Avg_W { get; set; }
}
