using Enms.Business.Workers.Abstractions;

namespace Enms.Business.Services;

public class MeterPushWorker(
) : BackgroundService, IWorker
{
  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    throw new NotImplementedException();
  }
}
