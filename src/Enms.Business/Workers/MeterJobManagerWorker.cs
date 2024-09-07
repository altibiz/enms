using System.Threading.Channels;
using Enms.Business.Conversion.Complex;
using Enms.Business.Models.Complex;
using Enms.Business.Workers.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities.Base;
using Enms.Data.Observers.Abstractions;
using Enms.Data.Observers.EventArgs;
using Enms.Jobs.Manager.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Workers;

public class MeterJobManagerWorker(
  IServiceScopeFactory serviceScopeFactory,
  IEntityChangesSubscriber subscriber
) : BackgroundService, IWorker
{
  private readonly Channel<EntitiesChangedEventArgs> channel =
    Channel.CreateUnbounded<EntitiesChangedEventArgs>();

  public override async Task StartAsync(CancellationToken cancellationToken)
  {
    await using (var scope = serviceScopeFactory.CreateAsyncScope())
    {
      var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
      var manager =
        scope.ServiceProvider.GetRequiredService<IMeterJobManager>();

      var meters = await context.Meters.ToListAsync(cancellationToken);
      foreach (var meter in meters)
      {
        await manager.EnsureInactivityMonitorJob(
          meter.Id,
          meter.MaxMaxInactivityPeriod.ToModel().ToTimeSpan()
        );
      }
    }

    subscriber.SubscribeEntitiesChanged(OnEntitiesChanged);

    await base.StartAsync(cancellationToken);
  }

  public override async Task StopAsync(CancellationToken cancellationToken)
  {
    subscriber.UnsubscribeEntitiesChanged(OnEntitiesChanged);

    await base.StopAsync(cancellationToken);
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    await foreach (var eventArgs in channel.Reader.ReadAllAsync(stoppingToken))
    {
      await using var scope = serviceScopeFactory.CreateAsyncScope();
      await Handle(scope.ServiceProvider, eventArgs);
    }
  }

  private void OnEntitiesChanged(
    object? sender,
    EntitiesChangedEventArgs eventArgs)
  {
    channel.Writer.TryWrite(eventArgs);
  }

  private static async Task Handle(
    IServiceProvider serviceProvider,
    EntitiesChangedEventArgs eventArgs)
  {
    var manager = serviceProvider.GetRequiredService<IMeterJobManager>();

    foreach (var entry in eventArgs.Entities)
    {
      if (entry.Entity is not MeterEntity meter)
      {
        continue;
      }

      if (entry.State is EntityChangedState.Added)
      {
        await manager.EnsureInactivityMonitorJob(
          meter.Id,
          meter.MaxMaxInactivityPeriod.ToModel().ToTimeSpan()
        );
      }

      if (entry.State is EntityChangedState.Removed)
      {
        await manager.UnscheduleInactivityMonitorJob(meter.Id);
      }

      if (entry.State is EntityChangedState.Modified)
      {
        await manager.RescheduleInactivityMonitorJob(
          meter.Id,
          meter.MaxMaxInactivityPeriod.ToModel().ToTimeSpan()
        );
      }
    }
  }
}
