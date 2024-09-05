using Enms.Business.Conversion.Base;
using Enms.Business.Models.Complex;
using Enms.Data.Entities.Complex;

namespace Enms.Business.Conversion.Complex;

public class
  LegalPersonModelEntityConverter : ModelEntityConverter<LegalPersonModel,
  LegalPersonEntity>
{
  protected override LegalPersonEntity ToEntity(LegalPersonModel model)
  {
    return model.ToEntity();
  }

  protected override LegalPersonModel ToModel(LegalPersonEntity entity)
  {
    return entity.ToModel();
  }
}

public static class LegalPersonModelEntityConverterExtensions
{
  public static LegalPersonEntity ToEntity(this LegalPersonModel model)
  {
    return new LegalPersonEntity
    {
      Name = model.Name,
      SocialSecurityNumber = model.SocialSecurityNumber,
      Address = model.Address,
      PostalCode = model.PostalCode,
      City = model.City,
      Email = model.Email,
      PhoneNumber = model.PhoneNumber
    };
  }

  public static LegalPersonModel ToModel(this LegalPersonEntity entity)
  {
    return new LegalPersonModel
    {
      Name = entity.Name,
      SocialSecurityNumber = entity.SocialSecurityNumber,
      Address = entity.Address,
      PostalCode = entity.PostalCode,
      City = entity.City,
      Email = entity.Email,
      PhoneNumber = entity.PhoneNumber
    };
  }
}
