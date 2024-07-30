using Enms.Fake.Client;
using Enms.Fake.Generators.Agnostic;

namespace Enms.Fake.Services;

public class PushHostedService(
  IServiceProvider serviceProvider
) : BackgroundService
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  protected override async Task ExecuteAsync(
    CancellationToken stoppingToken
  )
  {
    var push = _serviceProvider.GetRequiredService<PushOptions>();

    var now = DateTimeOffset.UtcNow;
    var lastPush = now;

    while (true)
    {
      if (stoppingToken.IsCancellationRequested)
      {
        return;
      }

      {
        await using var scope = _serviceProvider.CreateAsyncScope();

        now = DateTimeOffset.UtcNow;

        var pushClient =
          scope.ServiceProvider.GetRequiredService<EnmsPushClient>();
        var generator = scope.ServiceProvider
          .GetRequiredService<AgnosticMeasurementGenerator>();

        var measurements = (await generator.GenerateMeasurements(
          lastPush, now, push.MeterId, stoppingToken)).ToList();

        lastPush = now;

        foreach (var measurement in measurements)
        {
          var request = measurement;

          await pushClient.Push(
            push.MeterId,
            push.ApiKey,
            request,
            stoppingToken
          );
        }
      }

      var toWait =
        TimeSpan.FromSeconds(push.Interval_s)
        - (DateTimeOffset.UtcNow - now);
      if (toWait.TotalMilliseconds > 0)
      {
        await Task.Delay(toWait, stoppingToken);
      }
    }
  }
}
