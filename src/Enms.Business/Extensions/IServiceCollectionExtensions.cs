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
    services.AddObservers();

    // Queries
    services.AddScopedAssignableTo(typeof(IQueries));

    // Workers
    services.AddWorkers();

    return services;
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
      services.AddScoped(assignableTo, conversionType);
      services.AddScoped(conversionType);
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
      services.AddTransient(assignableTo, conversionType);
      services.AddTransient(conversionType);
    }
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
      services.AddSingleton(assignableTo, conversionType);
      services.AddSingleton(conversionType);
    }
  }

  private static void AddObservers(
    this IServiceCollection services
  )
  {
    var observerTypes = typeof(IServiceCollectionExtensions).Assembly
      .GetTypes()
      .Where(
        type =>
          !type.IsAbstract &&
          !type.IsGenericType &&
          type.IsClass &&
          type.IsAssignableTo(typeof(IPublisher)) &&
          type.IsAssignableTo(typeof(ISubscriber)));

    foreach (var observerType in observerTypes)
    {
      var publisherInterfaces = observerType.GetInterfaces()
        .Where(x => x.IsAssignableTo(typeof(IPublisher)));
      var subscriberInterfaces = observerType.GetInterfaces()
        .Where(x => x.IsAssignableTo(typeof(ISubscriber)));

      foreach (var publisherInterface in publisherInterfaces)
      {
        services.AddSingleton(publisherInterface, observerType);
      }

      foreach (var subscriberInterface in subscriberInterfaces)
      {
        services.AddSingleton(subscriberInterface, observerType);
      }
    }
  }

  private static void AddWorkers(
    this IServiceCollection services
  )
  {
    var workerTypes = typeof(IServiceCollectionExtensions).Assembly
      .GetTypes()
      .Where(
        type =>
          !type.IsAbstract &&
          !type.IsGenericType &&
          type.IsClass &&
          type.IsAssignableTo(typeof(IWorker)));

    foreach (var workerType in workerTypes)
    {
      services.AddSingleton(typeof(IHostedService), workerType);
    }
  }
}
