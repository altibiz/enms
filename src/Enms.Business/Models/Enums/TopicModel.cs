using Enms.Data.Entities.Enums;

namespace Enms.Business.Models.Enums;

public enum TopicModel
{
  All,
  System,
  Meter,
  MeterInactivity
}

public static class TopicModelExtensions
{
  public static TopicModel ToModel(this TopicEntity entity)
  {
    return entity switch
    {
      TopicEntity.All => TopicModel.All,
      TopicEntity.System => TopicModel.System,
      TopicEntity.Meter => TopicModel.Meter,
      TopicEntity.MeterInactivity => TopicModel.MeterInactivity,
      _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
    };
  }

  public static TopicEntity ToEntity(this TopicModel model)
  {
    return model switch
    {
      TopicModel.All => TopicEntity.All,
      TopicModel.System => TopicEntity.System,
      TopicModel.Meter => TopicEntity.Meter,
      TopicModel.MeterInactivity => TopicEntity.MeterInactivity,
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }

  public static string ToTitle(this TopicModel model)
  {
    return model switch
    {
      TopicModel.All => "General",
      TopicModel.Meter => "Messenger",
      TopicModel.MeterInactivity => "Messenger inactivity",
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }

  public static HashSet<TopicModel> ToTopics(this RoleModel model)
  {
    return model switch
    {
      RoleModel.OperatorRepresentative =>
      [
        TopicModel.All,
        TopicModel.Meter,
        TopicModel.MeterInactivity
      ],
      RoleModel.UserRepresentative => [],
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }
}
