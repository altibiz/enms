using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Base;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;

namespace Enms.Business.Conversion;

public class
  EventModelEntityConverter : ModelEntityConverter<EventModel, EventEntity>
{
  protected override EventEntity ToEntity(EventModel model)
  {
    return model.ToEntity();
  }

  protected override EventModel ToModel(EventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class EventModelEntityConverterExtensions
{
  public static EventEntity ToEntity(this EventModel model)
  {
    if (model is SystemEventModel systemEventModel)
    {
      return systemEventModel.ToEntity();
    }

    if (model is RepresentativeEventModel representativeEventModel)
    {
      return representativeEventModel.ToEntity();
    }

    if (model is MeterEventModel meterEventModel)
    {
      return meterEventModel.ToEntity();
    }

    if (model is AuditEventModel auditEventModel)
    {
      return auditEventModel.ToEntity();
    }

    return new EventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Content = model.Content,
      Categories = model.Categories.Select(x => x.ToEntity()).ToList()
    };
  }

  public static EventModel ToModel(this EventEntity entity)
  {
    if (entity is SystemEventEntity systemEventEntity)
    {
      return systemEventEntity.ToModel();
    }

    if (entity is RepresentativeEventEntity representativeEventEntity)
    {
      return representativeEventEntity.ToModel();
    }

    if (entity is MeterEventEntity meterEventEntity)
    {
      return meterEventEntity.ToModel();
    }

    if (entity is AuditEventEntity auditEventEntity)
    {
      return auditEventEntity.ToModel();
    }

    return new EventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Content = entity.Content,
      Categories = entity.Categories.Select(x => x.ToModel()).ToHashSet()
    };
  }
}
