using Enms.Business.Models.Abstractions;
using Enms.Data.Entities.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IModelEntityConverter
{
  bool CanConvertToEntity(Type modelType);

  bool CanConvertToModel(Type entityType);

  IEntity ToEntity(IModel model);

  IModel ToModel(IEntity entity);

  Type EntityType();
}
