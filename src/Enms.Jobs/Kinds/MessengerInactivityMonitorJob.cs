using Enms.Jobs.Observers.Abstractions;
using Quartz;

namespace Enms.Jobs.Kinds;

public class MeterInactivityMonitorJob(
  IMeterJobPublisher messengerJobPublisher
) : IJob
{
  public string Id { get; set; } = default!;

  public Task Execute(IJobExecutionContext context)
  {
    messengerJobPublisher.PublishInactivity(Id);

    return Task.CompletedTask;
  }
}
