using System.Reflection;
using Enms.Data.Concurrency;
using Enms.Data.Context;
using Enms.Data.Observers.Abstractions;
using Enms.Data.Options;
using Npgsql;

namespace Enms.Data.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsData(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.Configure<EnmsDataOptions>(
      builder.Configuration.GetSection("Enms:Data"));

    var dataOptions =
      builder.Configuration
        .GetSection("Enms:Data")
        .Get<EnmsDataOptions>()
      ?? throw new InvalidOperationException(
        "Enms:Data not found in configuration"
      );

    services.AddDbContextFactory<DataDbContext>(
      (services, options) =>
      {
        if (builder.Environment.IsDevelopment()
          && Environment.GetEnvironmentVariable("ENMS_LOG_SQL") is not null)
        {
          options.EnableSensitiveDataLogging();
          options.EnableDetailedErrors();
          options.UseLoggerFactory(
            LoggerFactory.Create(builder => builder.AddConsole())
          );
        }

        var dataSourceBuilder =
          new NpgsqlDataSourceBuilder(dataOptions.ConnectionString);
        dataSourceBuilder.ApplyConfigurationsFromAssembly(
          Assembly.GetExecutingAssembly());
        var dataSource = dataSourceBuilder.Build();

        options
          .UseTimescale(
            dataSource,
            options =>
            {
              options.MigrationsAssembly(
                typeof(DataDbContext).Assembly.GetName().Name);
              options.MigrationsHistoryTable(
                $"__Enms{nameof(DataDbContext)}");
            })
          .AddServedSaveChangesInterceptorsFromAssembly(
            Assembly.GetExecutingAssembly(),
            services
          );
      });

    services.AddScoped<DataDbContextMutex>();

    services.AddObservers();

    return services;
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
}
