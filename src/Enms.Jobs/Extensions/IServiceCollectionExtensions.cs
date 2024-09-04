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

    return services;
  }
}
