using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Complex;

namespace Enms.Business.Models.Base;

public class MeterModel : AuditableModel, IMeter
{
  [Required]
  public required PeriodModel MaxInactivityPeriod { get; set; }

  [Required]
  public required PeriodModel PushDelayPeriod { get; set; }
}
