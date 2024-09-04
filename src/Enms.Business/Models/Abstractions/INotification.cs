using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Abstractions;

public interface INotification : IIdentifiable
{
  public string Summary { get; }

  public string Content { get; }

  public DateTimeOffset Timestamp { get; }

  public string? EventId { get; }

  public HashSet<TopicModel> Topics { get; }
}