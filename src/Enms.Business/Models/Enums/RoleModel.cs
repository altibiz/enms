using Enms.Data.Entities.Enums;

namespace Enms.Business.Models.Enums;

public enum RoleModel
{
  OperatorRepresentative,

  UserRepresentative
}

public static class RoleModelExtensions
{
  public static RoleModel ToModel(this RoleEntity entity)
  {
    return entity switch
    {
      RoleEntity.OperatorRepresentative => RoleModel.OperatorRepresentative,
      RoleEntity.UserRepresentative => RoleModel.UserRepresentative,
      _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
    };
  }

  public static RoleEntity ToEntity(this RoleModel model)
  {
    return model switch
    {
      RoleModel.OperatorRepresentative => RoleEntity.OperatorRepresentative,
      RoleModel.UserRepresentative => RoleEntity.UserRepresentative,
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }
}
