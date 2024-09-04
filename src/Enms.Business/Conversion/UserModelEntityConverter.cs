using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Users.Entities;

namespace Enms.Business.Conversion;

public class
  UserModelEntityConverter : ModelEntityConverter<UserModel, UserEntity>
{
  protected override UserEntity ToEntity(UserModel model)
  {
    return model.ToEntity();
  }

  protected override UserModel ToModel(UserEntity entity)
  {
    return entity.ToModel();
  }
}

public static class UserModelEntityConverterExtensions
{
  public static UserEntity ToEntity(this UserModel model)
  {
    return new UserEntity(
      model.Id,
      model.UserName,
      model.Email
    );
  }

  public static UserModel ToModel(this UserEntity entity)
  {
    return new UserModel
    {
      Id = entity.Id,
      UserName = entity.UserName,
      Email = entity.Email
    };
  }
}
