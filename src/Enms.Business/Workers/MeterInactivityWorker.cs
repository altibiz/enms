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
using Enms.Data.Extensions;
using Enms.Jobs.Observers.Abstractions;
using Enms.Jobs.Observers.EventArgs;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Workers;

// TODO: handle null meter
// TODO: meter paging
// TODO: configurable inactivity duration

public class MeterInactivityWorker(
  IServiceScopeFactory serviceScopeFactory,
  IMeterJobSubscriber subscriber
) : BackgroundService, IWorker
{
  private static readonly JsonSerializerOptions
    EventContentSerializationOptions = new()
    {
      WriteIndented = true
    };

  private readonly Channel<MeterInactivityEventArgs> channel =
    Channel.CreateUnbounded<MeterInactivityEventArgs>();

  public override async Task StartAsync(CancellationToken cancellationToken)
  {
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
    await foreach (var eventArgs in channel.Reader.ReadAllAsync(stoppingToken))
    {
      await using var scope = serviceScopeFactory.CreateAsyncScope();
      await Handle(scope.ServiceProvider, eventArgs);
    }
  }

  private void OnInactivity(object? sender, MeterInactivityEventArgs eventArgs)
  {
    channel.Writer.TryWrite(eventArgs);
  }

  private static async Task Handle(
    IServiceProvider serviceProvider,
    MeterInactivityEventArgs eventArgs)
  {
    var sender = serviceProvider.GetRequiredService<INotificationSender>();
    var context = serviceProvider.GetRequiredService<DataDbContext>();

    var meter = (await context.Meters
        .FirstOrDefaultAsync(
          context.PrimaryKeyEquals<MeterEntity>(eventArgs.Id)))
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
