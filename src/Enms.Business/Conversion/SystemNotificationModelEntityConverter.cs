using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class SystemNotificationModelEntityConverter : ModelEntityConverter<
  SystemNotificationModel, SystemNotificationEntity>
{
  protected override SystemNotificationEntity ToEntity(SystemNotificationModel model)
  {
    return model.ToEntity();
  }

  protected override SystemNotificationModel ToModel(SystemNotificationEntity entity)
  {
    return entity.ToModel();
  }
}

public static class SystemNotificationModelEntityConverterExtensions
{
  public static SystemNotificationEntity ToEntity(
    this SystemNotificationModel model)
  {
    return new()
    {
      Id = model.Id,
      Title = model.Title,
      Summary = model.Summary,
      Content = model.Content,
      Topics = model.Topics.Select(x => x.ToEntity()).ToList(),
      EventId = model.EventId,
      Timestamp = model.Timestamp,
    };
  }

  public static SystemNotificationModel ToModel(
    this SystemNotificationEntity entity)
  {
    return new()
    {
      Id = entity.Id,
      Title = entity.Title,
      Summary = entity.Summary,
      Content = entity.Content,
      Topics = entity.Topics.Select(x => x.ToModel()).ToHashSet(),
      EventId = entity.EventId,
      Timestamp = entity.Timestamp,
    };
  }
}
