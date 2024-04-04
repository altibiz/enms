using System.Reflection;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Conversion.Agnostic;
using Enms.Fake.Client;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Conversion.Agnostic;
using Enms.Fake.Correction.Abstractions;
using Enms.Fake.Correction.Agnostic;
using Enms.Fake.Generators.Abstractions;
using Enms.Fake.Generators.Agnostic;
using Enms.Fake.Loaders;

// TODO: figure out how to discover generic types nicely

namespace Enms.Fake.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddGenerators(
    this IServiceCollection services)
  {
    services.AddTransientAssignableTo(typeof(IMeasurementGenerator));
    services.AddSingleton(typeof(AgnosticMeasurementGenerator));

    services.AddTransientAssignableTo(
      typeof(IPushRequestMeasurementConverter),
      typeof(IPushRequestMeasurementConverter).Assembly
    );
    services.AddSingleton(typeof(AgnosticPushRequestMeasurementConverter));

    services.AddTransientAssignableTo(
      typeof(IModelEntityConverter),
      typeof(IModelEntityConverter).Assembly
    );
    services.AddSingleton(typeof(AgnosticModelEntityConverter));

    return services;
  }

  public static IServiceCollection AddLoaders(this IServiceCollection services)
  {
    services.AddTransient(typeof(CsvLoader<>));
    services.AddSingleton(typeof(ResourceCache));
    return services;
  }

  public static IServiceCollection AddRecords(this IServiceCollection services)
  {
    services.AddTransientAssignableTo(
      typeof(IMeasurementRecordPushRequestConverter));
    services.AddSingleton(
      typeof(AgnosticMeasurementRecordPushRequestConverter));
    services.AddTransientAssignableTo(
      typeof(ICumulativeCorrector));
    services.AddSingleton(typeof(AgnosticCumulativeCorrector));
    return services;
  }

  public static IServiceCollection AddClient(
    this IServiceCollection services,
    int timeout,
    string baseUrl
  )
  {
    services.AddHttpClient(
      "Enms.Fake", options =>
      {
        options.Timeout = TimeSpan.FromSeconds(timeout);
        options.BaseAddress = new Uri(baseUrl);
      });
    services.AddScoped(typeof(EnmsPushClient));
    return services;
  }

  private static void AddTransientAssignableTo(
    this IServiceCollection services,
    Type assignableTo,
    Assembly? assembly = null
  )
  {
    assembly ??= typeof(IServiceCollectionExtensions).Assembly;
    var conversionTypes = assembly
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
