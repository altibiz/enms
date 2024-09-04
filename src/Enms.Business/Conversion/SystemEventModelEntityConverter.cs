using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class
  SystemEventModelEntityConverter : ModelEntityConverter<SystemEventModel,
  SystemEventEntity>
{
  protected override SystemEventEntity ToEntity(SystemEventModel model)
  {
    return model.ToEntity();
  }

  protected override SystemEventModel ToModel(SystemEventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class SystemEventModelExtensions
{
  public static SystemEventModel ToModel(this SystemEventEntity entity)
  {
    return new SystemEventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Content = entity.Content,
      Categories = entity.Categories.Select(c => c.ToModel()).ToHashSet()
    };
  }

  public static SystemEventEntity ToEntity(this SystemEventModel model)
  {
    return new SystemEventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Content = model.Content,
      Categories = model.Categories.Select(c => c.ToEntity()).ToList()
    };
  }
}
