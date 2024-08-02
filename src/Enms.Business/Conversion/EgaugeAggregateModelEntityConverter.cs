using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class EgaugeAggregateModelEntityConverter : ModelEntityConverter<
  EgaugeAggregateModel, EgaugeAggregateEntity>
{
  protected override EgaugeAggregateEntity ToEntity(
    EgaugeAggregateModel model)
  {
    return model.ToEntity();
  }

  protected override EgaugeAggregateModel ToModel(
    EgaugeAggregateEntity entity)
  {
    return entity.ToModel();
  }
}

public static class EgaugeAggregateModelExtensions
{
  public static EgaugeAggregateModel ToModel(
    this EgaugeAggregateEntity entity)
  {
    return new EgaugeAggregateModel
    {
      MeterId = entity.MeterId,
      LineId = entity.LineId,
      Timestamp = entity.Timestamp,
      Interval = entity.Interval.ToModel(),
      Count = entity.Count,
      VoltageL1AnyT0Avg_V = (decimal)entity.VoltageL1AnyT0Avg_V,
      VoltageL2AnyT0Avg_V = (decimal)entity.VoltageL2AnyT0Avg_V,
      VoltageL3AnyT0Avg_V = (decimal)entity.VoltageL3AnyT0Avg_V,
      CurrentL1AnyT0Avg_A = (decimal)entity.CurrentL1AnyT0Avg_A,
      CurrentL2AnyT0Avg_A = (decimal)entity.CurrentL2AnyT0Avg_A,
      CurrentL3AnyT0Avg_A = (decimal)entity.CurrentL3AnyT0Avg_A,
      ActivePowerL1NetT0Avg_W = (decimal)entity.ActivePowerL1NetT0Avg_W,
      ActivePowerL2NetT0Avg_W = (decimal)entity.ActivePowerL2NetT0Avg_W,
      ActivePowerL3NetT0Avg_W = (decimal)entity.ActivePowerL3NetT0Avg_W,
      ApparentPowerL1NetT0Avg_W = (decimal)entity.ApparentPowerL1NetT0Avg_W,
      ApparentPowerL2NetT0Avg_W = (decimal)entity.ApparentPowerL2NetT0Avg_W,
      ApparentPowerL3NetT0Avg_W = (decimal)entity.ApparentPowerL3NetT0Avg_W
    };
  }

  public static EgaugeAggregateEntity ToEntity(
    this EgaugeAggregateModel model)
  {
    return new EgaugeAggregateEntity
    {
      MeterId = model.MeterId,
      LineId = model.LineId,
      Timestamp = model.Timestamp,
      Interval = model.Interval.ToEntity(),
      Count = model.Count,
      VoltageL1AnyT0Avg_V = (float)model.VoltageL1AnyT0Avg_V,
      VoltageL2AnyT0Avg_V = (float)model.VoltageL2AnyT0Avg_V,
      VoltageL3AnyT0Avg_V = (float)model.VoltageL3AnyT0Avg_V,
      CurrentL1AnyT0Avg_A = (float)model.CurrentL1AnyT0Avg_A,
      CurrentL2AnyT0Avg_A = (float)model.CurrentL2AnyT0Avg_A,
      CurrentL3AnyT0Avg_A = (float)model.CurrentL3AnyT0Avg_A,
      ActivePowerL1NetT0Avg_W = (float)model.ActivePowerL1NetT0Avg_W,
      ActivePowerL2NetT0Avg_W = (float)model.ActivePowerL2NetT0Avg_W,
      ActivePowerL3NetT0Avg_W = (float)model.ActivePowerL3NetT0Avg_W,
      ApparentPowerL1NetT0Avg_W = (float)model.ApparentPowerL1NetT0Avg_W,
      ApparentPowerL2NetT0Avg_W = (float)model.ApparentPowerL2NetT0Avg_W,
      ApparentPowerL3NetT0Avg_W = (float)model.ApparentPowerL3NetT0Avg_W
    };
  }
}
