using Enms.Business.Conversion.Base;
using Enms.Business.Models.Complex;
using Enms.Business.Models.Enums;
using Enms.Data.Entities.Complex;

namespace Enms.Business.Conversion.Complex;

public class PeriodModelEntityConverter : ModelEntityConverter<
  PeriodModel, PeriodEntity>
{
  protected override PeriodEntity ToEntity(PeriodModel model)
  {
    return model.ToEntity();
  }

  protected override PeriodModel ToModel(PeriodEntity entity)
  {
    return entity.ToModel();
  }
}

public static class TimeSpanModelEntityConverterExtensions
{
  public static PeriodEntity ToEntity(this PeriodModel model)
  {
    return new PeriodEntity
    {
      Duration = model.Duration.ToEntity(),
      Multiplier = model.Multiplier
    };
  }

  public static PeriodModel ToModel(this PeriodEntity entity)
  {
    return new PeriodModel
    {
      Duration = entity.Duration.ToModel(),
      Multiplier = entity.Multiplier
    };
  }
}
