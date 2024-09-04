using System.Text;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Models.Joins;
using Enms.Business.Notifications.Abstractions;
using Enms.Data.Context;
using Enms.Email.Sender.Abstractions;

namespace Enms.Business.Notifications;

public class NotificationSender(
  DataDbContext context,
  AgnosticModelEntityConverter converter,
  IEmailSender sender
) : INotificationSender
{
  public async Task SendAsync(
    INotification notification,
    IEnumerable<RepresentativeModel> recipients)
  {
    context.Add(converter.ToEntity(notification));
    context.AddRange(
      recipients
        .Select(
          recipient => new NotificationRecipientModel
          {
            NotificationId = notification.Id,
            RepresentativeId = recipient.Id
          })
        .Select(converter.ToEntity));
    await context.SaveChangesAsync();

    var titleBuilder = new StringBuilder($"[ENMS]: {notification.Title}");
    if (notification.Topics.Count > 0)
    {
      var topics = notification.Topics.Select(x => x.ToTitle());
      titleBuilder.Append(" ( ");
      titleBuilder.Append(string.Join(", ", topics));
      titleBuilder.Append(" )");
    }

    await sender.SendBulkAsync(
      recipients.Select(
        recipient => new EmailMessage(
          recipient.PhysicalPerson.Name,
          recipient.PhysicalPerson.Email,
          titleBuilder.ToString(),
          notification.Content
        )
      ));
  }
}
