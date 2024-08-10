using Enms.Business.Conversion.Base;
using Enms.Business.Models.Base;
using Enms.Business.Models.Enums;
using Enms.Data.Entities.Base;

namespace Enms.Business.Conversion;

public class EventModelEntityConverter : ModelEntityConverter<EventModel, EventEntity>
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
    return new EventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Description = model.Description,
    };
  }

  public static EventModel ToModel(this EventEntity entity)
  {
    return new EventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Description = entity.Description,
    };
  }
}
