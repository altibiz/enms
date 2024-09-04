using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Enms.Business.Activation;
using Enms.Business.Conversion;
using Enms.Business.Models.Enums;
using Enms.Business.Notifications.Abstractions;
using Enms.Business.Workers.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;
using Enms.Data.Entities.Enums;
using Enms.Jobs.Manager.Abstractions;
using Enms.Jobs.Observers.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Services;

// TODO: handle null meter
// TODO: meter paging
// TODO: configurable inactivity duration

public class MeterInactivityWorker(
  IMeterJobSubscriber subscriber,
  IMeterJobManager manager,
  IServiceScopeFactory serviceScopeFactory
) : BackgroundService, IWorker
{
  private static readonly JsonSerializerOptions
    EventContentSerializationOptions = new()
    {
      WriteIndented = true
    };

  private readonly Channel<string> inactive =
    Channel.CreateUnbounded<string>();

  public override async Task StartAsync(CancellationToken cancellationToken)
  {
    await using (var scope = serviceScopeFactory.CreateAsyncScope())
    {
      var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
      var messengers = await context.Meters.ToListAsync();
      foreach (var messenger in messengers)
      {
        await manager.EnsureInactivityMonitorJob(
          messenger.Id,
          TimeSpan.FromMinutes(5)
        );
      }
    }

    subscriber.SubscribeInactivity(OnInactivity);
    await base.StartAsync(cancellationToken);
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    subscriber.UnsubscribeInactivity(OnInactivity);
    return base.StopAsync(cancellationToken);
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      var id = await inactive.Reader.ReadAsync(stoppingToken);
      await using var scope = serviceScopeFactory.CreateAsyncScope();
      await Notify(scope.ServiceProvider, id);
    }
  }

  private void OnInactivity(object? sender, string id)
  {
    inactive.Writer.TryWrite(id);
  }

  private async Task Notify(IServiceProvider serviceProvider, string id)
  {
    var sender = serviceProvider.GetRequiredService<INotificationSender>();
    var context = serviceProvider.GetRequiredService<DataDbContext>();

    var meter = (await context.Meters
        .FirstOrDefaultAsync(context.PrimaryKeyEquals<MeterEntity>(id)))
      ?.ToModel();

    if (meter is null)
    {
      return;
    }

    var lastPushEvent = await context.Events
      .OfType<MeterEventEntity>()
      .Where(x => x.MeterId == meter.Id)
      .Where(x => x.Categories.Contains(CategoryEntity.MeterPush))
      .OrderByDescending(x => x.Timestamp)
      .FirstOrDefaultAsync();

    var recipients = (await context.Representatives
        .Where(
          x => x.Topics.Contains(TopicEntity.All)
            || x.Topics.Contains(TopicEntity.Meter)
            || x.Topics.Contains(TopicEntity.MeterInactivity))
        .ToListAsync())
      .Select(x => x.ToModel());

    var notification = MeterNotificationModelActivator.New();
    notification.MeterId = meter.Id;
    notification.Topics =
    [
      TopicModel.All,
      TopicModel.Meter,
      TopicModel.MeterInactivity
    ];
    notification.Summary = $"Meter \"{meter.Title}\" is inactive";
    if (lastPushEvent is null)
    {
      notification.Content = "Meter never pushed";
    }
    else
    {
      var builder = new StringBuilder();
      var lastPushEventDetails = JsonSerializer.Serialize(
        lastPushEvent.Content,
        EventContentSerializationOptions
      );
      builder.AppendLine($"Meter: \"{meter.Title}\"");
      builder.AppendLine($"Last pushed at: {lastPushEvent.Timestamp}");
      builder.AppendLine($"Last push details: {lastPushEventDetails}");
      notification.Content = builder.ToString();
    }

    await sender.SendAsync(notification, recipients);
  }
}
