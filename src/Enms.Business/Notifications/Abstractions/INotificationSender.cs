using Enms.Business.Models;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Notifications.Abstractions;

public record NotificationRecipient(
  string Name,
  string Address
);

public interface INotificationSender
{
  Task SendAsync(
    INotification notification,
    IEnumerable<RepresentativeModel> recipients
  );
}