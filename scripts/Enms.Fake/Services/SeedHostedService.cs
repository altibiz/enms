using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models.Abstractions;
using Enms.Fake.Client;
using Enms.Fake.Generators.Agnostic;

// TODO: acclimate for egauge

namespace Enms.Fake.Services;

public class SeedHostedService(
  IServiceProvider serviceProvider
) : BackgroundService
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  protected override async Task ExecuteAsync(
    CancellationToken stoppingToken
  )
  {
    var seed = _serviceProvider.GetRequiredService<SeedOptions>();

    var now = DateTimeOffset.UtcNow;

    await using var scope = _serviceProvider.CreateAsyncScope();

    var generator = scope.ServiceProvider
      .GetRequiredService<AgnosticMeasurementGenerator>();
    var converter = scope.ServiceProvider
      .GetRequiredService<AgnosticPushRequestMeasurementConverter>();

    var seedTimeBegin = seed.Interval switch
    {
      SeedInterval.Hour => now.AddHours(-1),
      SeedInterval.Day => now.AddDays(-1),
      SeedInterval.Week => now.AddDays(-7),
      SeedInterval.Month => now.AddMonths(-1),
      SeedInterval.Season => now.AddMonths(-3),
      SeedInterval.Year => now.AddYears(-1),
      _ => throw new InvalidOperationException($"Unknown seed: {seed}")
    };

    while (seedTimeBegin < now)
    {
      var seedTimeEnd = seedTimeBegin.AddDays(1) > now
        ? now
        : seedTimeBegin.AddDays(1);

      var measurements = new List<IMeasurement>();
      foreach (var lineId in seed.LineIds)
      {
        measurements.AddRange(
          await generator.GenerateMeasurements(
            seedTimeBegin,
            seedTimeEnd,
            seed.MeterId,
            lineId,
            stoppingToken));
      }

      while (measurements.Count > 0)
      {
        var batch = measurements.Take(seed.BatchSize).ToList();
        measurements.RemoveRange(0, batch.Count);

        var request = converter.ToHttpContent(
          seed.MeterId, batch);

        var pushClient =
          scope.ServiceProvider.GetRequiredService<EnmsPushClient>();
        await pushClient.Push(
          seed.MeterId,
          seed.ApiKey,
          request,
          stoppingToken);
      }

      seedTimeBegin = seedTimeEnd;
    }

    Environment.Exit(0);
  }
}
