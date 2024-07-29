using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class
  EgaugeMeasurementModel : MeasurementModel<EgaugeMeasurementValidatorModel>
{
  [Required]
  public required decimal VoltageL1AnyT0_V { get; set; }

  [Required]
  public required decimal VoltageL2AnyT0_V { get; set; }

  [Required]
  public required decimal VoltageL3AnyT0_V { get; set; }

  [Required]
  public required decimal CurrentL1AnyT0_A { get; set; }

  [Required]
  public required decimal CurrentL2AnyT0_A { get; set; }

  [Required]
  public required decimal CurrentL3AnyT0_A { get; set; }

  [Required]
  public required decimal ActivePowerL1NetT0_W { get; set; }

  [Required]
  public required decimal ActivePowerL2NetT0_W { get; set; }

  [Required]
  public required decimal ActivePowerL3NetT0_W { get; set; }

  [Required]
  public required decimal ApparentPowerL1NetT0_W { get; set; }

  [Required]
  public required decimal ApparentPowerL2NetT0_W { get; set; }

  [Required]
  public required decimal ApparentPowerL3NetT0_W { get; set; }
}
