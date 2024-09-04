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
        .GetValue<EnmsJobsOptions>("Enms:Jobs")
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

    services.AddSingletonAssignableTo(typeof(IMeterJobManager));

    services.AddObservers();

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
}
