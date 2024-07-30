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
      LineId = entity.LineId,
      Timestamp = entity.Timestamp,
      VoltageL1AnyT0_V = (decimal)entity.VoltageL1AnyT0_V,
      VoltageL2AnyT0_V = (decimal)entity.VoltageL2AnyT0_V,
      VoltageL3AnyT0_V = (decimal)entity.VoltageL3AnyT0_V,
      CurrentL1AnyT0_A = (decimal)entity.CurrentL1AnyT0_A,
      CurrentL2AnyT0_A = (decimal)entity.CurrentL2AnyT0_A,
      CurrentL3AnyT0_A = (decimal)entity.CurrentL3AnyT0_A,
      ActivePowerL1NetT0_W = (decimal)entity.ActivePowerL1NetT0_W,
      ActivePowerL2NetT0_W = (decimal)entity.ActivePowerL2NetT0_W,
      ActivePowerL3NetT0_W = (decimal)entity.ActivePowerL3NetT0_W,
      ApparentPowerL1NetT0_W = (decimal)entity.ApparentPowerL1NetT0_W,
      ApparentPowerL2NetT0_W = (decimal)entity.ApparentPowerL2NetT0_W,
      ApparentPowerL3NetT0_W = (decimal)entity.ApparentPowerL3NetT0_W
    };
  }

  public static EgaugeMeasurementEntity ToEntity(
    this EgaugeMeasurementModel model)
  {
    return new EgaugeMeasurementEntity
    {
      LineId = model.LineId,
      Timestamp = model.Timestamp,
      VoltageL1AnyT0_V = (float)model.VoltageL1AnyT0_V,
      VoltageL2AnyT0_V = (float)model.VoltageL2AnyT0_V,
      VoltageL3AnyT0_V = (float)model.VoltageL3AnyT0_V,
      CurrentL1AnyT0_A = (float)model.CurrentL1AnyT0_A,
      CurrentL2AnyT0_A = (float)model.CurrentL2AnyT0_A,
      CurrentL3AnyT0_A = (float)model.CurrentL3AnyT0_A,
      ActivePowerL1NetT0_W = (float)model.ActivePowerL1NetT0_W,
      ActivePowerL2NetT0_W = (float)model.ActivePowerL2NetT0_W,
      ActivePowerL3NetT0_W = (float)model.ActivePowerL3NetT0_W,
      ApparentPowerL1NetT0_W = (float)model.ApparentPowerL1NetT0_W,
      ApparentPowerL2NetT0_W = (float)model.ApparentPowerL2NetT0_W,
      ApparentPowerL3NetT0_W = (float)model.ApparentPowerL3NetT0_W
    };
  }
}
