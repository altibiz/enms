using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Joins;

public class NotificationRecipientModel : IModel
{
  [Required]
  public required string NotificationId { get; set; } = default!;

  [Required]
  public required string RepresentativeId { get; set; } = default!;

  public DateTimeOffset? SeenOn { get; set; } = default!;
}
