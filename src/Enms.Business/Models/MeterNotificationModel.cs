using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class MeterNotificationModel : ResolvableNotificationModel
{
  [Required]
  public required string MeterId { get; set; } = default!;
}
