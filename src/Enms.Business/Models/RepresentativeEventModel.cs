using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class RepresentativeEventModel : EventModel
{
  [Required]
  public required string RepresentativeId { get; set; }
}
