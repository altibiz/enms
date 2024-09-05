using System.Linq.Expressions;
using Enms.Data.Context;
using Microsoft.EntityFrameworkCore;

// TODO: cache expressions/compilations
// TODO: check if conversion to string is needed for all properties

namespace Enms.Data.Extensions;

public static class DbContextKeyExtensions
{
  public static Func<T, string> PrimaryKeyOfCompiled<T>(this DbContext context)
  {
    return context.PrimaryKeyOf<T>().Compile();
  }

  public static Expression<Func<T, string>> PrimaryKeyOf<T>(
    this DbContext context)
  {
    var original = context.PrimaryKeyOfAgnostic(typeof(T));
    var parameter = Expression.Parameter(typeof(T));
    return Expression.Lambda<Func<T, string>>(
      Expression.Invoke(
        original,
        Expression.Convert(parameter, typeof(object))),
      parameter
    );
  }

  public static Func<object, string> PrimaryKeyOfAgnosticCompiled(
    this DbContext context,
    Type type)
  {
    return context.PrimaryKeyOfAgnostic(type).Compile();
  }

  public static Expression<Func<object, string>> PrimaryKeyOfAgnostic(
    this DbContext context,
    Type type)
  {
    var entityType = context.Model.FindEntityType(type)
      ?? throw new InvalidOperationException(
        $"No entity type found for {type}");
    var key = entityType.FindPrimaryKey()
      ?? throw new InvalidOperationException(
        $"No primary key found for {type}");
    var properties = key.Properties;
    var objectParameter = Expression.Parameter(typeof(object));
    var parameter = Expression.Convert(objectParameter, type);
    var propertyExpressions = properties
      .Select(
        property =>
          property.PropertyInfo is { } propertyInfo
            ? Expression.Property(parameter, propertyInfo)
            : property.FieldInfo is { } fieldInfo
              ? Expression.Field(parameter, fieldInfo)
              : throw new InvalidOperationException(
                $"No property or field found for {property}"));
    if (properties.Count == 1)
    {
      var propertyExpression = propertyExpressions.Single();
      var propertyToStringExpression =
        Expression.Convert(
          Expression.Convert(propertyExpression, typeof(object)),
          typeof(string));
      return Expression.Lambda<Func<object, string>>(
        propertyToStringExpression, objectParameter);
    }

    var stringJoinWithDashExpression = Expression.Call(
      typeof(string).GetMethod(
        nameof(string.Join),
        [typeof(string), typeof(string[])]) ??
      throw new InvalidOperationException(
        $"No {nameof(string.Join)} method found in {typeof(string)}"),
      Expression.Constant(DataDbContext.KeyJoin),
      Expression.NewArrayInit(typeof(string), propertyExpressions));

    return Expression.Lambda<Func<object, string>>(
      stringJoinWithDashExpression, objectParameter);
  }

  public static Func<T, string> PrimaryKeyOfCompiled<T, TConverted>(
    this DbContext context,
    Expression<Func<T, TConverted>> prefix)
  {
    return context.PrimaryKeyOf(prefix).Compile();
  }

  public static Expression<Func<T, string>> PrimaryKeyOf<T, TConverted>(
    this DbContext context,
    Expression<Func<T, TConverted>> prefix)
  {
    var parameter = Expression.Parameter(typeof(T));
    var callExpression = Expression.Invoke(prefix, parameter);
    var primaryKeyExpression = context.PrimaryKeyOf<TConverted>();
    return Expression.Lambda<Func<T, string>>(
      Expression.Invoke(primaryKeyExpression, callExpression), parameter);
  }

  public static Func<T, bool> PrimaryKeyEqualsCompiled<T>(
    this DbContext context,
    params string[] ids)
  {
    return context.PrimaryKeyEquals<T>(ids).Compile();
  }

  public static Expression<Func<T, bool>> PrimaryKeyEquals<T>(
    this DbContext context,
    params string[] ids)
  {
    var original = context.PrimaryKeyEqualsAgnostic(typeof(T), ids);
    var parameter = Expression.Parameter(typeof(T));
    return Expression.Lambda<Func<T, bool>>(
      Expression.Invoke(
        original,
        Expression.Convert(parameter, typeof(object))),
      parameter
    );
  }

  public static Func<object, bool> PrimaryKeyEqualsAgnosticCompiled(
    this DbContext context,
    Type type,
    params string[] ids)
  {
    return context.PrimaryKeyEqualsAgnostic(type, ids).Compile();
  }

  public static Expression<Func<object, bool>> PrimaryKeyEqualsAgnostic(
    this DbContext context,
    Type type,
    params string[] ids)
  {
    var objectParameter = Expression.Parameter(typeof(object));
    var parameter = Expression.Convert(objectParameter, type);
    var primaryKeyExpression = context.PrimaryKeyOfAgnostic(type);
    var primaryKeyEqualsExpression = Expression.Equal(
      Expression.Invoke(primaryKeyExpression, parameter),
      Expression.Constant(string.Join(DataDbContext.KeyJoin, ids)));
    return Expression.Lambda<Func<object, bool>>(
      primaryKeyEqualsExpression,
      objectParameter
    );
  }

  public static Func<T, bool> PrimaryKeyInCompiled<T>(
    this DbContext context,
    ICollection<string> ids)
  {
    return context.PrimaryKeyIn<T>(ids).Compile();
  }

  public static Expression<Func<T, bool>> PrimaryKeyIn<T>(
    this DbContext context,
    ICollection<string> ids)
  {
    var original = context.PrimaryKeyInAgnostic(typeof(T), ids);
    var parameter = Expression.Parameter(typeof(T));
    return Expression.Lambda<Func<T, bool>>(
      Expression.Invoke(
        original,
        Expression.Convert(parameter, typeof(object))),
      parameter
    );
  }

  public static Func<object, bool> PrimaryKeyInAgnosticCompiled(
    this DbContext context,
    Type type,
    ICollection<string> ids)
  {
    return context.PrimaryKeyInAgnostic(type, ids).Compile();
  }

  public static Expression<Func<object, bool>> PrimaryKeyInAgnostic(
    this DbContext context,
    Type type,
    ICollection<string> ids)
  {
    var objectParameter = Expression.Parameter(typeof(object));
    var parameter = Expression.Convert(objectParameter, type);
    var primaryKeyExpression = context.PrimaryKeyOfAgnostic(type);
    var primaryKeyInExpression = Expression.Call(
      Expression.Constant(ids),
      typeof(ICollection<string>).GetMethod(
        nameof(ICollection<string>.Contains)) ??
      throw new InvalidOperationException(
        $"No {
          nameof(ICollection<string>.Contains)
        } method found in {
          typeof(ICollection<string>)
        }"),
      Expression.Invoke(primaryKeyExpression, parameter));
    return Expression.Lambda<Func<object, bool>>(
      primaryKeyInExpression,
      objectParameter
    );
  }

  public static Func<T, string> ForeignKeyOfCompiled<T>(
    this DbContext context,
    string property)
  {
    return context.ForeignKeyOf<T>(property).Compile();
  }

  public static Expression<Func<T, string>> ForeignKeyOf<T>(
    this DbContext context,
    string property)
  {
    var original = context.ForeignKeyOfAgnostic(typeof(T), property);
    var parameter = Expression.Parameter(typeof(T));
    return Expression.Lambda<Func<T, string>>(
      Expression.Invoke(
        original,
        Expression.Convert(parameter, typeof(object))),
      parameter
    );
  }

  public static Func<T, bool> ForeignKeyEqualsCompiled<T>(
    this DbContext context,
    string property,
    params string[] ids)
  {
    return context.ForeignKeyEquals<T>(property, ids).Compile();
  }

  public static Expression<Func<T, bool>> ForeignKeyEquals<T>(
    this DbContext context,
    string property,
    params string[] ids)
  {
    var original = context.ForeignKeyEqualsAgnostic(typeof(T), property, ids);
    var parameter = Expression.Parameter(typeof(T));
    return Expression.Lambda<Func<T, bool>>(
      Expression.Invoke(
        original,
        Expression.Convert(parameter, typeof(object))),
      parameter
    );
  }

  public static Func<object, bool> ForeignKeyEqualsAgnosticCompiled(
    this DbContext context,
    Type type,
    string property,
    params string[] ids)
  {
    return context.ForeignKeyEqualsAgnostic(type, property, ids).Compile();
  }

  public static Expression<Func<object, bool>> ForeignKeyEqualsAgnostic(
    this DbContext context,
    Type type,
    string property,
    params string[] ids)
  {
    var objectParameter = Expression.Parameter(typeof(object));
    var parameter = Expression.Convert(objectParameter, type);
    var foreignKeyExpression = context.ForeignKeyOfAgnostic(type, property);
    var foreignKeyEqualsExpression = Expression.Equal(
      Expression.Invoke(foreignKeyExpression, parameter),
      Expression.Constant(string.Join(DataDbContext.KeyJoin, ids)));
    return Expression.Lambda<Func<object, bool>>(
      foreignKeyEqualsExpression,
      objectParameter
    );
  }

  public static Func<object, string> ForeignKeyOfAgnosticCompiled(
    this DbContext context,
    Type type,
    string property)
  {
    return context.ForeignKeyOfAgnostic(type, property).Compile();
  }

  public static Expression<Func<object, string>> ForeignKeyOfAgnostic(
    this DbContext context,
    Type type,
    string property)
  {
    var entityType = context.Model.FindEntityType(type)
      ?? throw new InvalidOperationException(
        $"No entity type found for {type}");
    var navigation = entityType.FindNavigation(property)
      ?? throw new InvalidOperationException(
        $"No navigation found for {type} {property}");
    var foreignKey = navigation.ForeignKey ??
      throw new InvalidOperationException(
        $"No foreign key found for {type} {property}");
    var properties = foreignKey.Properties;
    var objectParameter = Expression.Parameter(typeof(object));
    var parameter = Expression.Convert(objectParameter, type);
    var propertyExpressions = properties
      .Select(
        property =>
          property.PropertyInfo is { } propertyInfo
            ? Expression.Property(parameter, propertyInfo)
            : property.FieldInfo is { } fieldInfo
              ? Expression.Field(parameter, fieldInfo)
              : throw new InvalidOperationException(
                $"No property or field found for {property}"));
    if (properties.Count == 1)
    {
      var propertyExpression = propertyExpressions.Single();
      var propertyToStringExpression =
        Expression.Convert(
          Expression.Convert(propertyExpression, typeof(object)),
          typeof(string));
      return Expression.Lambda<Func<object, string>>(
        propertyToStringExpression, objectParameter);
    }

    var stringJoinWithDashExpression = Expression.Call(
      typeof(string).GetMethod(
        nameof(string.Join),
        [typeof(string), typeof(string[])]) ??
      throw new InvalidOperationException(
        $"No {nameof(string.Join)} method found in {typeof(string)}"),
      Expression.Constant(DataDbContext.KeyJoin),
      Expression.NewArrayInit(typeof(string), propertyExpressions));
    return Expression.Lambda<Func<object, string>>(
      stringJoinWithDashExpression, objectParameter);
  }

  public static Func<T, string> ForeignKeyOfCompiled<T, TConverted>(
    this DbContext context,
    Expression<Func<T, TConverted>> prefix,
    string property)
  {
    return context.ForeignKeyOf(prefix, property).Compile();
  }

  public static Expression<Func<T, string>> ForeignKeyOf<T, TConverted>(
    this DbContext context,
    Expression<Func<T, TConverted>> prefix,
    string property)
  {
    var original = context.ForeignKeyOfAgnostic(prefix, property);
    var parameter = Expression.Parameter(typeof(T));
    return Expression.Lambda<Func<T, string>>(
      Expression.Invoke(
        original,
        Expression.Convert(parameter, typeof(object))),
      parameter
    );
  }

  public static Func<object, string>
    ForeignKeyOfAgnosticCompiled<T, TConverted>(
      this DbContext context,
      Expression<Func<T, TConverted>> prefix,
      string property
    )
  {
    return context.ForeignKeyOfAgnostic(prefix, property).Compile();
  }

  public static Expression<Func<object, string>> ForeignKeyOfAgnostic<T,
    TConverted>(
    this DbContext context,
    Expression<Func<T, TConverted>> prefix,
    string property
  )
  {
    var objectParameter = Expression.Parameter(typeof(object));
    var parameter = Expression.Convert(objectParameter, typeof(T));
    var callExpression = Expression.Invoke(prefix, parameter);
    var foreignKeyExpression = context.ForeignKeyOf<TConverted>(property);
    return Expression.Lambda<Func<object, string>>(
      Expression.Invoke(foreignKeyExpression, callExpression),
      objectParameter
    );
  }
}
