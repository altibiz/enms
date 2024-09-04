using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Base;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;

namespace Enms.Business.Conversion;

public class ResolvableNotificationModelEntityConverter : ModelEntityConverter<
  ResolvableNotificationModel, ResolvableNotificationEntity>
{
  protected override ResolvableNotificationEntity ToEntity(
    ResolvableNotificationModel model)
  {
    return model.ToEntity();
  }

  protected override ResolvableNotificationModel ToModel(
    ResolvableNotificationEntity entity)
  {
    return entity.ToModel();
  }
}

public static class ResolvableNotificationModelEntityConverterExtensions
{
  public static ResolvableNotificationEntity ToEntity(
    this ResolvableNotificationModel model)
  {
    if (model is MeterNotificationModel meterNotificationModel)
    {
      return meterNotificationModel.ToEntity();
    }

    return new ResolvableNotificationEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      EventId = model.EventId,
      Summary = model.Summary,
      Content = model.Content,
      Topics = model.Topics.Select(x => x.ToEntity()).ToList(),
      ResolvedById = model.ResolvedById,
      ResolvedOn = model.ResolvedOn
    };
  }

  public static ResolvableNotificationModel ToModel(
    this ResolvableNotificationEntity entity)
  {
    if (entity is MeterNotificationEntity meterNotificationEntity)
    {
      return meterNotificationEntity.ToModel();
    }

    return new ResolvableNotificationModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      EventId = entity.EventId,
      Summary = entity.Summary,
      Content = entity.Content,
      Topics = entity.Topics.Select(x => x.ToModel()).ToHashSet(),
      ResolvedById = entity.ResolvedById,
      ResolvedOn = entity.ResolvedOn
    };
  }
}
