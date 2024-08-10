using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class
  EgaugeLineModelEntityConverter : ModelEntityConverter<EgaugeLineModel,
  EgaugeLineEntity>
{
  protected override EgaugeLineEntity ToEntity(EgaugeLineModel model)
  {
    return model.ToEntity();
  }

  protected override EgaugeLineModel ToModel(EgaugeLineEntity entity)
  {
    return entity.ToModel();
  }
}

public static class EgaugeLineModelEntityConverterExtensions
{
  public static EgaugeLineEntity ToEntity(this EgaugeLineModel model)
  {
    return new EgaugeLineEntity
    {
      Id = model.Id,
      LineId = model.LineId,
      MeterId = model.MeterId,
      Title = model.Title,
      CreatedOn = model.CreatedOn,
      CreatedById = model.CreatedById,
      LastUpdatedOn = model.LastUpdatedOn,
      LastUpdatedById = model.LastUpdatedById,
      IsDeleted = model.IsDeleted,
      DeletedOn = model.DeletedOn,
      DeletedById = model.DeletedById,
      MeasurementValidatorId = model.MeasurementValidatorId,
      ConnectionPower_W = model.ConnectionPower_W,
      Phases = model.Phases.Select(p => p.ToEntity()).ToList()
    };
  }

  public static EgaugeLineModel ToModel(this EgaugeLineEntity entity)
  {
    return new EgaugeLineModel
    {
      Id = entity.Id,
      LineId = entity.LineId,
      MeterId = entity.MeterId,
      Title = entity.Title,
      CreatedOn = entity.CreatedOn,
      CreatedById = entity.CreatedById,
      LastUpdatedOn = entity.LastUpdatedOn,
      LastUpdatedById = entity.LastUpdatedById,
      IsDeleted = entity.IsDeleted,
      DeletedOn = entity.DeletedOn,
      DeletedById = entity.DeletedById,
      MeasurementValidatorId = entity.MeasurementValidatorId,
      ConnectionPower_W = entity.ConnectionPower_W,
      Phases = entity.Phases.Select(p => p.ToModel()).ToHashSet()
    };
  }
}
