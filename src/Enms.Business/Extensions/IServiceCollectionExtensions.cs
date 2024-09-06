using Enms.Business.Activation.Abstractions;
using Enms.Business.Activation.Agnostic;
using Enms.Business.Aggregation.Abstractions;
using Enms.Business.Aggregation.Agnostic;
using Enms.Business.Capabilities.Abstractions;
using Enms.Business.Capabilities.Agnostic;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Localization.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Business.Naming.Abstractions;
using Enms.Business.Naming.Agnostic;
using Enms.Business.Notifications.Abstractions;
using Enms.Business.Observers.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Business.Workers.Abstractions;

namespace Enms.Business.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsBusiness(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    // Activation
    services.AddTransientAssignableTo(typeof(IModelActivator));
    services.AddSingleton(typeof(AgnosticModelActivator));

    // Aggregation
    services.AddTransientAssignableTo(typeof(IAggregateUpserter));
    services.AddSingleton(typeof(AgnosticAggregateUpserter));

    // Capabilities
    services.AddTransientAssignableTo(typeof(ILineCapabilities));
    services.AddSingleton(typeof(AgnosticLineCapabilities));

    // Conversion
    services.AddTransientAssignableTo(typeof(IModelEntityConverter));
    services.AddSingleton(typeof(AgnosticModelEntityConverter));
    services.AddTransientAssignableTo(typeof(IMeasurementAggregateConverter));
    services.AddSingleton(typeof(AgnosticMeasurementAggregateConverter));
    services.AddTransientAssignableTo(typeof(IPushRequestMeasurementConverter));
    services.AddSingleton(typeof(AgnosticPushRequestMeasurementConverter));

    // Localization
    services.AddSingletonAssignableTo(typeof(ILocalizer));

    // Mutations
    services.AddScopedAssignableTo(typeof(IMutations));

    // Naming
    services.AddTransientAssignableTo(typeof(ILineNamingConvention));
    services.AddSingleton(typeof(AgnosticLineNamingConvention));

    // Notifications
    services.AddSingletonAssignableTo(typeof(INotificationSender));

    // Observers
    services.AddSingletonAssignableTo(typeof(IPublisher));
    services.AddSingletonAssignableTo(typeof(ISubscriber));

    // Queries
    services.AddScopedAssignableTo(typeof(IQueries));

    // Workers
    services.AddSingletonAssignableTo(typeof(IWorker));

    return services;
  }

  private static void AddSingletonAssignableTo(
    this IServiceCollection services,
    Type assignableTo
  )
  {
    var conversionTypes = typeof(IServiceCollectionExtensions).Assembly
      .GetTypes()
      .Where(
        type =>
          !type.IsAbstract &&
          !type.IsGenericType &&
          type.IsClass &&
          type.IsAssignableTo(assignableTo));

    foreach (var conversionType in conversionTypes)
    {
      services.AddSingleton(conversionType);
      foreach (var interfaceType in conversionType.GetInterfaces())
      {
        services.AddSingleton(interfaceType, services =>
          services.GetRequiredService(conversionType));
      }
    }
  }

  private static void AddScopedAssignableTo(
    this IServiceCollection services,
    Type assignableTo
  )
  {
    var conversionTypes = typeof(IServiceCollectionExtensions).Assembly
      .GetTypes()
      .Where(
        type =>
          !type.IsAbstract &&
          !type.IsGenericType &&
          type.IsClass &&
          type.IsAssignableTo(assignableTo));

    foreach (var conversionType in conversionTypes)
    {
      services.AddScoped(conversionType);
      foreach (var interfaceType in conversionType.GetAllInterfaces())
      {
        services.AddScoped(interfaceType, services =>
          services.GetRequiredService(conversionType));
      }
    }
  }

  private static void AddTransientAssignableTo(
    this IServiceCollection services,
    Type assignableTo
  )
  {
    var conversionTypes = typeof(IServiceCollectionExtensions).Assembly
      .GetTypes()
      .Where(
        type =>
          !type.IsAbstract &&
          !type.IsGenericType &&
          type.IsClass &&
          type.IsAssignableTo(assignableTo));

    foreach (var conversionType in conversionTypes)
    {
      services.AddTransient(conversionType);
      foreach (var interfaceType in conversionType.GetAllInterfaces())
      {
        services.AddTransient(interfaceType, services =>
          services.GetRequiredService(conversionType));
      }
    }
  }

  private static Type[] GetAllInterfaces(this Type type)
  {
    return type.GetInterfaces()
      .Concat(type.GetInterfaces().SelectMany(GetAllInterfaces))
      .Concat(type.BaseType?.GetAllInterfaces() ?? Array.Empty<Type>())
      .ToHashSet()
      .ToArray();
  }
}
