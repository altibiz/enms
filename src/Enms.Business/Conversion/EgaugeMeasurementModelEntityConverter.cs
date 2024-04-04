using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class EgaugeMeasurementModelEntityConverter : ModelEntityConverter<
  EgaugeMeasurementModel, EgaugeMeasurementEntity>
{
  protected override EgaugeMeasurementEntity ToEntity(
    EgaugeMeasurementModel model)
  {
    return model.ToEntity();
  }

  protected override EgaugeMeasurementModel ToModel(
    EgaugeMeasurementEntity entity)
  {
    return entity.ToModel();
  }
}

public static class EgaugeMeasurementModelExtensions
{
  public static EgaugeMeasurementModel ToModel(
    this EgaugeMeasurementEntity entity)
  {
    return new EgaugeMeasurementModel
    {
      MeterId = entity.MeterId,
      Timestamp = entity.Timestamp,
      Voltage_V = entity.Voltage_V,
      Power_W = entity.Power_W
    };
  }

  public static EgaugeMeasurementEntity ToEntity(
    this EgaugeMeasurementModel model)
  {
    return new EgaugeMeasurementEntity
    {
      MeterId = model.MeterId,
      Timestamp = model.Timestamp,
      Voltage_V = model.Voltage_V,
      Power_W = model.Power_W
    };
  }
}
