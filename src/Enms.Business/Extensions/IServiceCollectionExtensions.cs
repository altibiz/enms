using System.Reflection;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Iot;
using Enms.Business.Mutations.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Extensions;

namespace Enms.Business.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsBusinessClient(
    this IServiceCollection services
  )
  {
    services.AddDbContextFactory<EnmsDataDbContext>(
      (services, options) => options
        .UseTimescale(
          services.GetRequiredService<IConfiguration>()
            .GetConnectionString("Enms") ?? throw new InvalidOperationException(
            "Enms connection string not found")
        )
        .AddServedSaveChangesInterceptorsFromAssembly(
          Assembly.GetExecutingAssembly(),
          services
        )
    );

    services.AddScopedAssignableTo(typeof(IEnmsQueries));
    services.AddScopedAssignableTo(typeof(IEnmsMutations));

    services.AddTransientAssignableTo(typeof(IAggregateUpserter));
    services.AddTransientAssignableTo(typeof(IModelEntityConverter));
    services.AddTransientAssignableTo(typeof(IMeasurementAggregateConverter));
    services.AddTransientAssignableTo(typeof(IPushRequestMeasurementConverter));

    services.AddScoped<EnmsIotHandler>();

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
      services.AddScoped(assignableTo, conversionType);
      services.AddTransient(conversionType);
    }
  }
}
