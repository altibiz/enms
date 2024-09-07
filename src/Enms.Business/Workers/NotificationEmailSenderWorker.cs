using System.Text;
using System.Threading.Channels;
using Enms.Business.Conversion;
using Enms.Business.Conversion.Joins;
using Enms.Business.Models.Enums;
using Enms.Business.Workers.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;
using Enms.Data.Entities.Joins;
using Enms.Data.Extensions;
using Enms.Data.Observers.Abstractions;
using Enms.Data.Observers.EventArgs;
using Enms.Email.Sender.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Workers;

// TODO: paging when fetching representatives
// TODO: use a join when fetching representatives
// TODO: fetch notification by joining with notification recipients

public class NotificationEmailSenderWorker(
  IServiceScopeFactory serviceScopeFactory,
  IEntityChangesSubscriber subscriber
) : BackgroundService, IWorker
{
  private readonly Channel<EntitiesChangedEventArgs> channel =
    Channel.CreateUnbounded<EntitiesChangedEventArgs>();

  public override async Task StartAsync(CancellationToken cancellationToken)
  {
    subscriber.SubscribeEntitiesChanged(OnChanged);

    await base.StartAsync(cancellationToken);
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    subscriber.UnsubscribeEntitiesChanged(OnChanged);

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

  private void OnChanged(object? sender, EntitiesChangedEventArgs eventArgs)
  {
    channel.Writer.TryWrite(eventArgs);
  }

  private static async Task Handle(
    IServiceProvider serviceProvider,
    EntitiesChangedEventArgs eventArgs)
  {
    var sender = serviceProvider.GetRequiredService<IEmailSender>();
    var context = serviceProvider.GetRequiredService<DataDbContext>();

    var notifications = eventArgs.Entities
      .Where(x => x.State == EntityChangedState.Added)
      .Select(x => x.Entity)
      .OfType<NotificationEntity>()
      .Select(x => x.ToModel());

    var recipients = eventArgs.Entities
      .Where(x => x.State == EntityChangedState.Added)
      .Select(x => x.Entity)
      .OfType<NotificationRecipientEntity>()
      .Select(x => x.ToModel());

    var representatives = await context.Representatives
      .Where(context.PrimaryKeyIn<RepresentativeEntity>(
        recipients
          .Select(x => x.RepresentativeId)
          .ToList()))
      .ToListAsync();

    var groups = recipients
      .GroupBy(x => x.NotificationId)
      .Select(x => new
      {
        Notification = notifications.First(y => y.Id == x.Key),
        Recipients = x.ToList(),
        Representatives = x
          .Select(y => representatives
            .FirstOrDefault(z => z.Id == y.RepresentativeId))
            .OfType<RepresentativeEntity>()
            .ToList()
      });

    var emails = new List<EmailMessage>();
    foreach (var group in groups)
    {
      if (group.Notification is null)
      {
        continue;
      }

      var notification = group.Notification;
      var titleBuilder = new StringBuilder($"[ENMS]: {notification.Title}");
      if (notification.Topics.Count > 0)
      {
        var topics = notification.Topics.Select(x => x.ToTitle());
        titleBuilder.Append(" ( ");
        titleBuilder.Append(string.Join(", ", topics));
        titleBuilder.Append(" )");
      }

      emails.AddRange(
        group.Representatives.Select(
          representative => new EmailMessage(
            representative.PhysicalPerson.Name,
            representative.PhysicalPerson.Email,
            titleBuilder.ToString(),
            notification.Content
          )
        ));
    }

    await sender.SendBulkAsync(emails);
  }
}
