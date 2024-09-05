using System.Reflection;
using Enms.Data.Concurrency;
using Enms.Data.Context;
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

    return services;
  }
}
