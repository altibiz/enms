using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Base;

public class NotificationModel : IdentifiableModel, INotification
{
  [Required]
  public required string Summary { get; set; } = default!;

  [Required]
  public required string Content { get; set; } = default!;

  [Required]
  public required DateTimeOffset Timestamp { get; set; }

  public string? EventId { get; set; } = default!;

  [Required]
  public required HashSet<TopicModel> Topics { get; set; } = default!;
}
