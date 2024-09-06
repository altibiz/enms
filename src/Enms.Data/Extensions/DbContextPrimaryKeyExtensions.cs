using System.Collections.Concurrent;
using System.Linq.Expressions;
using Enms.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Enms.Data.Extensions;

public static class DbContextPrimaryKeyExtensions
{
  public static Func<T, object> PrimaryKeyOfCompiled<T>(
    this DbContext context)
  {
    var typeBasedFunc = context.PrimaryKeyOfCompiled(typeof(T));
    return entity => typeBasedFunc(entity!);
  }

  public static Func<T, bool> PrimaryKeyEqualsCompiled<T>(
    this DbContext context,
    string id)
  {
    var typeBasedFunc = context.PrimaryKeyEqualsCompiled(typeof(T), id);
    return entity => typeBasedFunc(entity!);
  }

  public static Expression<Func<T, object>> PrimaryKeyOf<T>(
    this DbContext context)
  {
    var typeBasedExpr = context.PrimaryKeyOf(typeof(T));
    var parameter = Expression.Parameter(typeof(T), "entity");
    var cast = Expression.Convert(parameter, typeof(object));
    var body = Expression.Invoke(typeBasedExpr, cast);
    return Expression.Lambda<Func<T, object>>(body, parameter);
  }

  public static Expression<Func<T, bool>> PrimaryKeyEquals<T>(
    this DbContext context,
    string id)
  {
    var typeBasedExpr = context.PrimaryKeyEquals(typeof(T), id);
    var parameter = Expression.Parameter(typeof(T), "entity");
    var cast = Expression.Convert(parameter, typeof(object));
    var body = Expression.Invoke(typeBasedExpr, cast);
    return Expression.Lambda<Func<T, bool>>(body, parameter);
  }

  public static Func<object, object> PrimaryKeyOfCompiled(
    this DbContext context, Type entityType)
  {
    var key = (context.GetType(), entityType);

    if (_primaryKeyGetterCompiledCache.TryGetValue(key, out var cachedFunc))
    {
      return (Func<object, object>)cachedFunc;
    }

    var expr = context.PrimaryKeyOfUncached(entityType);
    var compiled = expr.Compile();
    _primaryKeyGetterCompiledCache[key] = compiled;

    return compiled;
  }

  public static Expression<Func<object, object>> PrimaryKeyOf(
    this DbContext context, Type entityType)
  {
    var key = (context.GetType(), entityType);

    if (_primaryKeyGetterExpressionCache.TryGetValue(key, out var cachedExpr))
    {
      return (Expression<Func<object, object>>)cachedExpr;
    }

    var expr = context.PrimaryKeyOfUncached(entityType);
    _primaryKeyGetterExpressionCache[key] = expr;

    return expr;
  }

  public static Func<object, bool> PrimaryKeyEqualsCompiled(
    this DbContext context, Type entityType, string id)
  {
    var key = (context.GetType(), entityType);

    if (_primaryKeyEqualsCompiledCache.TryGetValue(key, out var cachedFunc))
    {
      return (Func<object, bool>)cachedFunc;
    }

    var expr = context.PrimaryKeyEqualsUncached(entityType, id);
    var compiled = expr.Compile();
    _primaryKeyEqualsCompiledCache[key] = compiled;

    return compiled;
  }

  public static Expression<Func<object, bool>> PrimaryKeyEquals(
    this DbContext context, Type entityType, string id)
  {
    var key = (context.GetType(), entityType);

    if (_primaryKeyEqualsExpressionCache.TryGetValue(key, out var cachedExpr))
    {
      return (Expression<Func<object, bool>>)cachedExpr;
    }

    var expr = context.PrimaryKeyEqualsUncached(entityType, id);
    _primaryKeyEqualsExpressionCache[key] = expr;

    return expr;
  }

  private static Expression<Func<object, object>> PrimaryKeyOfUncached(
    this DbContext context, Type entityType)
  {
    var keyProperties = context.GetPrimaryKeyProperties(entityType);
    var parameter = Expression.Parameter(typeof(object), "e");
    var convertedParameter = Expression.Convert(parameter, entityType);

    var propertyExpressions = keyProperties
      .Select(p => Expression.Property(convertedParameter, p.PropertyInfo!))
      .ToList();

    Expression resultExpression;
    if (propertyExpressions.Count == 1)
    {
      resultExpression = propertyExpressions.Single();
    }
    else
    {
      var tupleType = typeof(ValueTuple<>).MakeGenericType(
        propertyExpressions.Select(p => p.Type).ToArray());
      var constructor = tupleType.GetConstructors().Single();

      resultExpression = Expression.New(
        constructor,
        propertyExpressions
      );
    }

    return Expression.Lambda<Func<object, object>>(
      Expression.Convert(resultExpression, typeof(object)),
      parameter);
  }

  private static Expression<Func<object, bool>> PrimaryKeyEqualsUncached(
    this DbContext context, Type entityType, string id)
  {
    var keyProperties = context.GetPrimaryKeyProperties(entityType);
    var idParts = id.Split(DataDbContext.KeyJoin);

    if (keyProperties.Count != idParts.Length)
    {
      throw new ArgumentException(
        "The number of ids must match the number of key properties.");
    }

    var parameter = Expression.Parameter(typeof(object), "e");
    var convertedParameter = Expression.Convert(parameter, entityType);

    Expression? equalityExpression = null;

    foreach (var (property, idValue) in keyProperties.Zip(idParts))
    {
      var propertyExpression =
        Expression.Property(convertedParameter, property.PropertyInfo!);
      var convertedId = Expression.Constant(
        Convert.ChangeType(idValue, property.ClrType));

      var equalsExpression = Expression.Equal(propertyExpression, convertedId);

      equalityExpression = equalityExpression == null
        ? equalsExpression
        : Expression.AndAlso(equalityExpression, equalsExpression);
    }

    return Expression.Lambda<Func<object, bool>>(equalityExpression!, parameter);
  }

  private static IReadOnlyList<IProperty> GetPrimaryKeyProperties(
    this DbContext context, Type entityType)
  {
    var entityTypeInfo = context.Model.FindEntityType(entityType)
      ?? throw new InvalidOperationException(
        $"No entity type found for {entityType}");
    var key = entityTypeInfo.FindPrimaryKey() ??
      throw new InvalidOperationException(
        $"No primary key found for {entityType}");
    return key.Properties;
  }

  private static readonly
    ConcurrentDictionary<(Type dbContextType, Type entityType), Delegate>
    _primaryKeyGetterCompiledCache = new();

  private static readonly
    ConcurrentDictionary<(Type dbContextType, Type entityType), Delegate>
    _primaryKeyEqualsCompiledCache = new();

  private static readonly
    ConcurrentDictionary<(Type dbContextType, Type entityType), Expression>
    _primaryKeyGetterExpressionCache = new();

  private static readonly
    ConcurrentDictionary<(Type dbContextType, Type entityType), Expression>
    _primaryKeyEqualsExpressionCache = new();
}
