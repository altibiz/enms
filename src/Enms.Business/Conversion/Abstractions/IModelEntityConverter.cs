using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IModelEntityConverter
{
  bool CanConvertToEntity(Type modelType);

  bool CanConvertToModel(Type entityType);

  object ToEntity(IModel model);

  IModel ToModel(object entity);

  Type EntityType();
}
