using Enms.Business.Conversion.Base;
using Enms.Business.Models.Complex;
using Enms.Data.Entities.Complex;

namespace Enms.Business.Conversion.Complex;

public class PhysicalPersonModelEntityConverter : ModelEntityConverter<
  PhysicalPersonModel, PhysicalPersonEntity>
{
  protected override PhysicalPersonEntity ToEntity(PhysicalPersonModel model)
  {
    return model.ToEntity();
  }

  protected override PhysicalPersonModel ToModel(PhysicalPersonEntity entity)
  {
    return entity.ToModel();
  }
}

public static class PhysicalPersonModelEntityConverterExtensions
{
  public static PhysicalPersonModel ToModel(this PhysicalPersonEntity entity)
  {
    return new PhysicalPersonModel
    {
      Name = entity.Name,
      Email = entity.Email,
      PhoneNumber = entity.PhoneNumber
    };
  }

  public static PhysicalPersonEntity ToEntity(this PhysicalPersonModel model)
  {
    return new PhysicalPersonEntity
    {
      Name = model.Name,
      Email = model.Email,
      PhoneNumber = model.PhoneNumber
    };
  }
}
