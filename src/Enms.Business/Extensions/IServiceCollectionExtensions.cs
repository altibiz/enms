using System.Reflection;
using Enms.Business.Aggregation;
using Enms.Business.Aggregation.Agnostic;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Iot;
using Enms.Business.Mutations.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Data;
using Enms.Data.Extensions;

namespace Enms.Business.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsBusinessClient(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.AddData(builder);

    services.AddScopedAssignableTo(typeof(IEnmsQueries));
    services.AddScopedAssignableTo(typeof(IEnmsMutations));

    services.AddTransientAssignableTo(typeof(IAggregateUpserter));
    services.AddSingleton(typeof(AgnosticAggregateUpserter));
    services.AddTransientAssignableTo(typeof(IModelEntityConverter));
    services.AddSingleton(typeof(AgnosticModelEntityConverter));
    services.AddTransientAssignableTo(typeof(IMeasurementAggregateConverter));
    services.AddSingleton(typeof(AgnosticMeasurementAggregateConverter));
    services.AddTransientAssignableTo(typeof(IPushRequestMeasurementConverter));
    services.AddSingleton(typeof(AgnosticPushRequestMeasurementConverter));

    services.AddScoped<EnmsIotHandler>();
    services.AddScoped<BatchAggregatedMeasurementUpserter>();

    return services;
  }

  private static void AddData(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.AddDbContextFactory<EnmsDataDbContext>(
      (services, options) =>
      {
        var connectionString = services
            .GetRequiredService<IConfiguration>()
            .GetConnectionString("Enms")
          ?? throw new InvalidOperationException(
            "Enms connection string not found");

        if (builder.Environment.IsDevelopment())
        {
#pragma warning disable S125
          // TODO: switch to enable query/mutation logging
          // options.EnableSensitiveDataLogging();
          // options.EnableDetailedErrors();
          // options.UseLoggerFactory(
          //   LoggerFactory.Create(builder => builder.AddConsole())
          // );
#pragma warning restore S125
        }

        options
          .UseTimescale(
            connectionString,
            options =>
            {
              options.MigrationsAssembly(
                typeof(EnmsDataDbContext).Assembly.GetName().Name);
              options.MigrationsHistoryTable(
                $"__{nameof(EnmsDataDbContext)}");
            })
          .AddServedSaveChangesInterceptorsFromAssembly(
            Assembly.GetExecutingAssembly(),
            services
          );
      });
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
