using Enms.Jobs.Observers.Abstractions;
using Enms.Jobs.Observers.EventArgs;
using Quartz;

namespace Enms.Jobs.Kinds;

public class MeterInactivityMonitorJob(
  IMeterJobPublisher messengerJobPublisher
) : IJob
{
  public string Id { get; set; } = default!;

  public Task Execute(IJobExecutionContext context)
  {
    messengerJobPublisher.PublishInactivity(
      new MeterInactivityEventArgs { Id = Id });

    return Task.CompletedTask;
  }
}
