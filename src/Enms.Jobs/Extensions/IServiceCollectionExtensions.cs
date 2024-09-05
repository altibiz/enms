using Enms.Jobs.Context;
using Enms.Jobs.Manager.Abstractions;
using Enms.Jobs.Observers.Abstractions;
using Enms.Jobs.Options;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Enms.Jobs.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsJobs(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    // Options
    services.Configure<EnmsJobsOptions>(
      builder.Configuration.GetSection("Enms:Jobs"));

    // Quartz
    services.AddQuartz(builder);

    // Managers
    services.AddSingletonAssignableTo(typeof(IJobManager));

    // Observers
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
      foreach (var interfaceType in conversionType.GetAllInterfaces())
      {
        services.AddSingleton(interfaceType, conversionType);
      }
    }
  }

  private static void AddQuartz(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
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

    services.AddDbContext<JobsDbContext>(
      options =>
      {
        options.UseNpgsql(jobsOptions.ConnectionString, x =>
        {
          x.MigrationsAssembly(
            typeof(JobsDbContext).Assembly.GetName().Name);
          x.MigrationsHistoryTable(
            $"__Enms{nameof(JobsDbContext)}");
        });
      });
  }

  private static Type[] GetAllInterfaces(this Type type)
  {
    return type.GetInterfaces()
      .Concat(type.GetInterfaces().SelectMany(GetAllInterfaces))
      .Concat(type.BaseType?.GetAllInterfaces() ?? Array.Empty<Type>())
      .ToHashSet()
      .ToArray();
  }
}
