using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class MeterEventModelEntityConverter : ModelEntityConverter<
  MeterEventModel, MeterEventEntity>
{
  protected override MeterEventEntity ToEntity(MeterEventModel model)
  {
    return model.ToEntity();
  }

  protected override MeterEventModel ToModel(MeterEventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class MeterEventModelEntityConverterExtensions
{
  public static MeterEventEntity ToEntity(this MeterEventModel model)
  {
    return new MeterEventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Content = model.Content,
      MeterId = model.MeterId,
      Categories = model.Categories.Select(c => c.ToEntity()).ToList()
    };
  }

  public static MeterEventModel ToModel(this MeterEventEntity entity)
  {
    return new MeterEventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Content = entity.Content,
      MeterId = entity.MeterId,
      Categories = entity.Categories.Select(c => c.ToModel()).ToHashSet()
    };
  }
}
