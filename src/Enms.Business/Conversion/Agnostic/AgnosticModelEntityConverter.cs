using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Agnostic;

public class AgnosticModelEntityConverter(IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public Type EntityType(Type type)
  {
    return _serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvertToEntity(type))
        ?.EntityType()
      ?? throw new InvalidOperationException(
        $"No converter found for model {type}.");
  }

  public object ToEntity(IModel model)
  {
    return _serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvertToEntity(model.GetType()))
        ?.ToEntity(model)
      ?? throw new InvalidOperationException(
        $"No converter found for model {model.GetType()}.");
  }

  public TEntity ToEntity<TEntity>(IModel model)
    where TEntity : class
  {
    return ToEntity(model) as TEntity
      ?? throw new InvalidOperationException(
        $"No converter found for model {model.GetType()}.");
  }

  public IModel ToModel(object entity)
  {
    return _serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvertToModel(entity.GetType()))
        ?.ToModel(entity)
      ?? throw new InvalidOperationException(
        $"No converter found for entity {entity.GetType()}.");
  }

  public TModel ToModel<TModel>(object entity)
    where TModel : class, IModel
  {
    return ToModel(entity) as TModel
      ?? throw new InvalidOperationException(
        $"No converter found for entity {entity.GetType()}.");
  }
}
