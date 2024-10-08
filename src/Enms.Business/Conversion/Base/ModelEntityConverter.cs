using System.ComponentModel;
using System.Globalization;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Base;

public abstract class ModelEntityConverter<TModel, TEntity> : TypeConverter,
  IModelEntityConverter
  where TModel : class, IModel
  where TEntity : class
{
  public bool CanConvertToEntity(Type modelType)
  {
    return modelType.IsAssignableTo(typeof(TModel));
  }

  public bool CanConvertToModel(Type entityType)
  {
    return entityType.IsAssignableTo(typeof(TEntity));
  }

  public object ToEntity(IModel model)
  {
    return ToEntity(
      model as TModel ??
      throw new ArgumentNullException(nameof(model)));
  }

  public IModel ToModel(object entity)
  {
    return ToModel(
      entity as TEntity ??
      throw new ArgumentNullException(nameof(entity)));
  }

  public Type EntityType()
  {
    return typeof(TEntity);
  }

  protected abstract TEntity ToEntity(TModel model);

  protected abstract TModel ToModel(TEntity entity);

  public override bool CanConvertFrom(
    ITypeDescriptorContext? context,
    Type sourceType)
  {
    return sourceType == typeof(TModel) ||
      base.CanConvertFrom(context, sourceType);
  }

  public override bool CanConvertTo(
    ITypeDescriptorContext? context,
    Type? destinationType)
  {
    return destinationType == typeof(TEntity) ||
      base.CanConvertTo(context, destinationType);
  }

  public override object? ConvertFrom(
    ITypeDescriptorContext? context,
    CultureInfo? culture,
    object value)
  {
    return value is TModel model
      ? ToEntity(model)
      : base.ConvertFrom(context, culture, value);
  }

  public override object? ConvertTo(
    ITypeDescriptorContext? context,
    CultureInfo? culture,
    object? value,
    Type destinationType)
  {
    return value is TEntity entity
      ? ToModel(entity)
      : base.ConvertTo(context, culture, value, destinationType);
  }
}
