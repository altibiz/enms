using Enms.Business.Conversion.Base;
using Enms.Business.Models.Base;
using Enms.Business.Models.Enums;
using Enms.Data.Entities.Base;

namespace Enms.Business.Conversion;

public class NotificationModelEntityConverter : ModelEntityConverter<
  NotificationModel, NotificationEntity>
{
  protected override NotificationEntity ToEntity(NotificationModel model)
  {
    return model.ToEntity();
  }

  protected override NotificationModel ToModel(NotificationEntity entity)
  {
    return entity.ToModel();
  }
}

public static class NotificationModelEntityConverterExtensions
{
  public static NotificationEntity ToEntity(this NotificationModel model)
  {
    if (model is ResolvableNotificationModel resolvable)
    {
      return resolvable.ToEntity();
    }

    return new NotificationEntity
    {
      Id = model.Id,
      Title = model.Title,
      Summary = model.Summary,
      Content = model.Content,
      Timestamp = model.Timestamp,
      EventId = model.EventId,
      Topics = model.Topics.Select(x => x.ToEntity()).ToList()
    };
  }

  public static NotificationModel ToModel(this NotificationEntity entity)
  {
    if (entity is ResolvableNotificationEntity resolvable)
    {
      return resolvable.ToModel();
    }

    return new NotificationModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Summary = entity.Summary,
      Content = entity.Content,
      Timestamp = entity.Timestamp,
      EventId = entity.EventId,
      Topics = entity.Topics.Select(x => x.ToModel()).ToHashSet()
    };
  }
}
