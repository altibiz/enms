using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class MeterNotificationModelEntityConverter : ModelEntityConverter<
  MeterNotificationModel, MeterNotificationEntity>
{
  protected override MeterNotificationEntity ToEntity(
    MeterNotificationModel model)
  {
    return model.ToEntity();
  }

  protected override MeterNotificationModel ToModel(
    MeterNotificationEntity entity)
  {
    return entity.ToModel();
  }
}

public static class MeterNotificationModelEntityConverterExtensions
{
  public static MeterNotificationEntity ToEntity(
    this MeterNotificationModel model)
  {
    return new MeterNotificationEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      EventId = model.EventId,
      Summary = model.Summary,
      Content = model.Content,
      Topics = model.Topics.Select(x => x.ToEntity()).ToList(),
      MeterId = model.MeterId,
      ResolvedById = model.ResolvedById,
      ResolvedOn = model.ResolvedOn
    };
  }

  public static MeterNotificationModel ToModel(
    this MeterNotificationEntity entity)
  {
    return new MeterNotificationModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      EventId = entity.EventId,
      Summary = entity.Summary,
      Content = entity.Content,
      Topics = entity.Topics.Select(x => x.ToModel()).ToHashSet(),
      MeterId = entity.MeterId,
      ResolvedById = entity.ResolvedById,
      ResolvedOn = entity.ResolvedOn
    };
  }
}
