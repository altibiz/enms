using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;
using Enms.Business.Models.Complex;

namespace Enms.Business.Models;

public class NetworkUserModel : AuditableModel
{
  [Required]
  public required LegalPersonModel LegalPerson { get; set; } = default!;
}
