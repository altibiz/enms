using Enms.Business.Conversion.Base;
using Enms.Business.Models.Complex;
using Enms.Business.Models.Enums;
using Enms.Data.Entities.Complex;

namespace Enms.Business.Conversion.Complex;

public class TimeSpanModelEntityConverter : ModelEntityConverter<
  TimeSpanModel, TimeSpanEntity>
{
  protected override TimeSpanEntity ToEntity(TimeSpanModel model)
  {
    return model.ToEntity();
  }

  protected override TimeSpanModel ToModel(TimeSpanEntity entity)
  {
    return entity.ToModel();
  }
}

public static class TimeSpanModelEntityConverterExtensions
{
  public static TimeSpanEntity ToEntity(this TimeSpanModel model)
  {
    return new TimeSpanEntity
    {
      Duration = model.Duration.ToEntity(),
      Multiplier = model.Multiplier
    };
  }

  public static TimeSpanModel ToModel(this TimeSpanEntity entity)
  {
    return new TimeSpanModel
    {
      Duration = entity.Duration.ToModel(),
      Multiplier = entity.Multiplier
    };
  }
}
