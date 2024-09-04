using System.Reflection;
using Enms.Data.Concurrency;
using Enms.Data.Context;

namespace Enms.Data.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsData(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.AddDbContextFactory<DataDbContext>(
      (services, options) =>
      {
        var connectionString = services
            .GetRequiredService<IConfiguration>()
            .GetConnectionString("Enms")
          ?? throw new InvalidOperationException(
            "Enms connection string not found");

        if (builder.Environment.IsDevelopment()
          && Environment.GetEnvironmentVariable("ENMS_LOG_SQL") is not null)
        {
          options.EnableSensitiveDataLogging();
          options.EnableDetailedErrors();
          options.UseLoggerFactory(
            LoggerFactory.Create(builder => builder.AddConsole())
          );
        }

        options
          .UseTimescale(
            connectionString,
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
