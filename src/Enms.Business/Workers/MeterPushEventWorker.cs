using System.Text.Json;
using System.Threading.Channels;
using Enms.Business.Conversion;
using Enms.Business.Models.Base;
using Enms.Business.Observers.Abstractions;
using Enms.Business.Observers.EventArgs;
using Enms.Business.Workers.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;
using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Workers;

public class MeterPushEventWorker(
  IServiceScopeFactory serviceScopeFactory,
  IMeasurementSubscriber subscriber
) : BackgroundService, IWorker
{
  private readonly Channel<MeasurementPushEventArgs> channel =
    Channel.CreateUnbounded<MeasurementPushEventArgs>();

  public override async Task StartAsync(CancellationToken cancellationToken)
  {
    subscriber.SubscribePush(OnPush);

    await base.StartAsync(cancellationToken);
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    subscriber.UnsubscribePush(OnPush);

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

  private void OnPush(object? sender, MeasurementPushEventArgs eventArgs)
  {
    channel.Writer.TryWrite(eventArgs);
  }

  private static async Task Handle(
    IServiceProvider serviceProvider,
    MeasurementPushEventArgs eventArgs)
  {
    var context = serviceProvider.GetRequiredService<DataDbContext>();

    var meterIds = eventArgs.Measurements.Select(x => x.MeterId).Distinct().ToList();
    var meters = await context.Meters
        .Where(context.PrimaryKeyIn<MeterEntity>(meterIds))
        .Select(x => x.ToModel())
        .ToListAsync();

    var events = meters.Select(
      meter => new MeterEventEntity
      {
        Title = $"Meter \"{meter.Title}\" has pushed",
        Timestamp = DateTimeOffset.UtcNow,
        MeterId = meter.Id,
        Level = LevelEntity.Info,
        Content = CreateEventContent(eventArgs, meters),
        Categories = [CategoryEntity.All, CategoryEntity.Meter, CategoryEntity.MeterPush],
      });

    context.AddRange(events);
    await context.SaveChangesAsync();
  }

  private static JsonDocument CreateEventContent(
    MeasurementPushEventArgs eventArgs,
    List<MeterModel> meters)
  {
    var content = new EventContent(
      meters.Count,
      meters.Select(
        meter => new EventContentLine(
          meter.Id,
          eventArgs.Measurements
            .Count(x => x.MeterId == meter.Id)
        )
      ).ToArray()
    );

    return JsonDocument.Parse(JsonSerializer.Serialize(content));
  }

  private sealed record EventContent(
    int Count,
    EventContentLine[] Lines
  );

  private sealed record EventContentLine(
    string Id,
    int Count
  );
}
