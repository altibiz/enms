using Enms.Business.Conversion.Base;
using Enms.Business.Conversion.Complex;
using Enms.Business.Models;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class
  EgaugeMeterModelEntityConverter : ModelEntityConverter<EgaugeMeterModel,
  EgaugeMeterEntity>
{
  protected override EgaugeMeterEntity ToEntity(EgaugeMeterModel model)
  {
    return model.ToEntity();
  }

  protected override EgaugeMeterModel ToModel(EgaugeMeterEntity entity)
  {
    return entity.ToModel();
  }
}

public static class EgaugeMeterModelEntityConverterExtensions
{
  public static EgaugeMeterEntity ToEntity(this EgaugeMeterModel model)
  {
    return new EgaugeMeterEntity
    {
      Id = model.Id,
      Title = model.Title,
      CreatedOn = model.CreatedOn,
      CreatedById = model.CreatedById,
      LastUpdatedOn = model.LastUpdatedOn,
      LastUpdatedById = model.LastUpdatedById,
      IsDeleted = model.IsDeleted,
      DeletedOn = model.DeletedOn,
      DeletedById = model.DeletedById,
      MaxMaxInactivityPeriod = model.MaxInactivityPeriod.ToEntity(),
      PushDelayPeriod = model.PushDelayPeriod.ToEntity()
    };
  }

  public static EgaugeMeterModel ToModel(this EgaugeMeterEntity entity)
  {
    return new EgaugeMeterModel
    {
      Id = entity.Id,
      Title = entity.Title,
      CreatedOn = entity.CreatedOn,
      CreatedById = entity.CreatedById,
      LastUpdatedOn = entity.LastUpdatedOn,
      LastUpdatedById = entity.LastUpdatedById,
      IsDeleted = entity.IsDeleted,
      DeletedOn = entity.DeletedOn,
      DeletedById = entity.DeletedById,
      MaxInactivityPeriod = entity.MaxMaxInactivityPeriod.ToModel(),
      PushDelayPeriod = entity.PushDelayPeriod.ToModel()
    };
  }
}
