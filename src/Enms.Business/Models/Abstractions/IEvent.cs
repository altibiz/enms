using System.Text.Json;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Abstractions;

public interface IEvent : IIdentifiable, IReadonly
{
  public DateTimeOffset Timestamp { get; }

  public LevelModel Level { get; }

  public JsonDocument Content { get; }

  public HashSet<CategoryModel> Categories { get; }
}
