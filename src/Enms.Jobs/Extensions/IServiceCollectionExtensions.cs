using Enms.Jobs.Manager.Abstractions;
using Enms.Jobs.Observers.Abstractions;
using Enms.Jobs.Options;
using Quartz;

namespace Enms.Jobs.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsJobs(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.Configure<EnmsJobsOptions>(
      builder.Configuration.GetSection("Enms:Jobs"));

    var jobsOptions = builder.Configuration
        .GetSection("Enms:Jobs")
        .Get<EnmsJobsOptions>()
      ?? throw new InvalidOperationException(
        "Missing Enms:Jobs configuration");

    services.AddQuartz(
      options =>
      {
        options.UsePersistentStore(
          options =>
          {
            options.UseSystemTextJsonSerializer();
            options.UsePostgres(
              options =>
              {
                options.ConnectionString = jobsOptions.ConnectionString;
              });
          });
      });

    services.AddQuartzHostedService(
      options =>
      {
        options.WaitForJobsToComplete = true;
        options.AwaitApplicationStarted = true;
      });

    services.AddSingletonAssignableTo(typeof(IJobManager));

    services.AddSingletonAssignableTo(typeof(IPublisher));
    services.AddSingletonAssignableTo(typeof(ISubscriber));

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
      foreach (var interfaceType in conversionType.GetInterfaces())
      {
        services.AddSingleton(interfaceType, conversionType);
      }
    }
  }
}
