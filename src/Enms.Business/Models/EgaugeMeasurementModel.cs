using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class
  EgaugeMeasurementModel : MeasurementModel<EgaugeMeasurementValidatorModel>
{
  [Required]
  public required float Voltage_V { get; init; }

  [Required]
  public required float Power_W { get; init; }
}
