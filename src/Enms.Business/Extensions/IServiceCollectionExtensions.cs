using Enms.Business.Activation.Abstractions;
using Enms.Business.Activation.Agnostic;
using Enms.Business.Aggregation.Abstractions;
using Enms.Business.Aggregation.Agnostic;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Localization;
using Enms.Business.Localization.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Business.Naming.Abstractions;
using Enms.Business.Naming.Agnostic;
using Enms.Business.Pushing;
using Enms.Business.Pushing.Abstractions;
using Enms.Business.Queries.Abstractions;

namespace Enms.Business.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsBusiness(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.AddScopedAssignableTo(typeof(IQueries));
    services.AddScopedAssignableTo(typeof(IMutations));

    services.AddTransientAssignableTo(typeof(IAggregateUpserter));
    services.AddSingleton(typeof(AgnosticAggregateUpserter));
    services.AddTransientAssignableTo(typeof(IModelEntityConverter));
    services.AddSingleton(typeof(AgnosticModelEntityConverter));
    services.AddTransientAssignableTo(typeof(IMeasurementAggregateConverter));
    services.AddSingleton(typeof(AgnosticMeasurementAggregateConverter));
    services.AddTransientAssignableTo(typeof(IPushRequestMeasurementConverter));
    services.AddSingleton(typeof(AgnosticPushRequestMeasurementConverter));
    services.AddTransientAssignableTo(typeof(IModelActivator));
    services.AddSingleton(typeof(AgnosticModelActivator));
    services.AddTransientAssignableTo(typeof(ILineNamingConvention));
    services.AddSingleton(typeof(AgnosticLineNamingConvention));

    services.AddScoped<IMeasurementPusher, MeasurementPusher>();
    services.AddSingleton<MeasurementPublisher>();
    services.AddSingleton<IMeasurementPublisher>(
      services => services
        .GetRequiredService<MeasurementPublisher>());
    services.AddSingleton<IMeasurementSubscriber>(
      services => services
        .GetRequiredService<MeasurementPublisher>());

    services.AddSingleton<ILocalizer, Localizer>();

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
}
