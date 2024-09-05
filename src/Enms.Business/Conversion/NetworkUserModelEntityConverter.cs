using Enms.Business.Conversion.Base;
using Enms.Business.Conversion.Complex;
using Enms.Business.Models;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class NetworkUserModelEntityConverter : ModelEntityConverter<NetworkUserModel,
  NetworkUserEntity>
{
  protected override NetworkUserEntity ToEntity(NetworkUserModel model)
  {
    return model.ToEntity();
  }

  protected override NetworkUserModel ToModel(NetworkUserEntity entity)
  {
    return entity.ToModel();
  }
}

public static class NetworkUserModelEntityConverterExtensions
{
  public static NetworkUserEntity ToEntity(this NetworkUserModel model)
  {
    return new NetworkUserEntity
    {
      Id = model.Id,
      Title = model.Title,
      CreatedOn = model.CreatedOn,
      CreatedById = model.CreatedById,
      LastUpdatedOn = model.LastUpdatedOn,
      LastUpdatedById = model.LastUpdatedById,
      IsDeleted = model.IsDeleted,
      DeletedOn = model.DeletedOn,
      DeletedById = model.DeletedById,
      LegalPerson = model.LegalPerson.ToEntity(),
    };
  }

  public static NetworkUserModel ToModel(this NetworkUserEntity entity)
  {
    return new NetworkUserModel
    {
      Id = entity.Id,
      Title = entity.Title,
      CreatedOn = entity.CreatedOn,
      CreatedById = entity.CreatedById,
      LastUpdatedOn = entity.LastUpdatedOn,
      LastUpdatedById = entity.LastUpdatedById,
      IsDeleted = entity.IsDeleted,
      DeletedOn = entity.DeletedOn,
      DeletedById = entity.DeletedById,
      LegalPerson = entity.LegalPerson.ToModel(),
    };
  }
}
