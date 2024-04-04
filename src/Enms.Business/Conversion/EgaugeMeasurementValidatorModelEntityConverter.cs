using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class EgaugeMeasurementValidatorModelEntityConverter :
  ModelEntityConverter<EgaugeMeasurementValidatorModel,
    EgaugeMeasurementValidatorEntity>
{
  protected override EgaugeMeasurementValidatorEntity ToEntity(
    EgaugeMeasurementValidatorModel model)
  {
    return model.ToEntity();
  }

  protected override EgaugeMeasurementValidatorModel ToModel(
    EgaugeMeasurementValidatorEntity entity)
  {
    return entity.ToModel();
  }
}

public static class EgaugeMeasurementValidatorModelEntityConverterExtensions
{
  public static EgaugeMeasurementValidatorModel ToModel(
    this EgaugeMeasurementValidatorEntity entity)
  {
    return new EgaugeMeasurementValidatorModel
    {
      Id = entity.Id,
      Title = entity.Title,
      CreatedOn = entity.CreatedOn,
      CreatedById = entity.CreatedById,
      LastUpdatedOn = entity.LastUpdatedOn,
      LastUpdatedById = entity.LastUpdatedById,
      IsDeleted = entity.IsDeleted,
      DeletedOn = entity.DeletedOn,
      DeletedById = entity.DeletedById
    };
  }

  public static EgaugeMeasurementValidatorEntity ToEntity(
    this EgaugeMeasurementValidatorModel model)
  {
    return new EgaugeMeasurementValidatorEntity
    {
      Id = model.Id,
      Title = model.Title,
      CreatedOn = model.CreatedOn,
      CreatedById = model.CreatedById,
      LastUpdatedOn = model.LastUpdatedOn,
      LastUpdatedById = model.LastUpdatedById,
      IsDeleted = model.IsDeleted,
      DeletedOn = model.DeletedOn,
      DeletedById = model.DeletedById
    };
  }
}
