using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Base;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;

namespace Enms.Business.Conversion;

public class
  MeterModelEntityConverter : ModelEntityConverter<MeterModel, MeterEntity>
{
  protected override MeterEntity ToEntity(MeterModel model)
  {
    return model.ToEntity();
  }

  protected override MeterModel ToModel(MeterEntity entity)
  {
    return entity.ToModel();
  }
}

public static class MeterModelEntityConverterExtensions
{
  public static MeterEntity ToEntity(this MeterModel model)
  {
    if (model is EgaugeMeterModel egaugeModel)
    {
      return egaugeModel.ToEntity();
    }

    return new MeterEntity
    {
      Id = model.Id,
      Title = model.Title,
      CreatedOn = model.CreatedOn,
      CreatedById = model.CreatedById,
      LastUpdatedOn = model.LastUpdatedOn,
      LastUpdatedById = model.LastUpdatedById,
      IsDeleted = model.IsDeleted,
      DeletedById = model.DeletedById,
      DeletedOn = model.DeletedOn
    };
  }

  public static MeterModel ToModel(this MeterEntity entity)
  {
    if (entity is EgaugeMeterEntity egaugeEntity)
    {
      return egaugeEntity.ToModel();
    }

    return new MeterModel
    {
      Id = entity.Id,
      Title = entity.Title,
      CreatedOn = entity.CreatedOn,
      CreatedById = entity.CreatedById,
      LastUpdatedOn = entity.LastUpdatedOn,
      LastUpdatedById = entity.LastUpdatedById,
      IsDeleted = entity.IsDeleted,
      DeletedById = entity.DeletedById,
      DeletedOn = entity.DeletedOn
    };
  }
}
