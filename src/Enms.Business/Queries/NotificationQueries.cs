using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Base;
using Enms.Business.Queries.Abstractions;
using Enms.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Queries;

public class NotificationQueries(
  DataDbContext context,
  IServiceProvider serviceProvider
) : IQueries
{
  public async Task<PaginatedList<NotificationModel>> Read(
    string recipientId = default!,
    bool includeSeen = false,
    int pageNumber = QueryConstants.StartingPage,
    int pageCount = QueryConstants.DefaultPageCount
  )
  {
    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(typeof(NotificationModel)))
      ?? throw new InvalidOperationException(
        $"No model entity converter found for model {typeof(NotificationModel)}");

    var filtered = recipientId is null
      ? context.Notifications
      : includeSeen
        ? context.NotificationRecipients
            .Where(x => x.RecipientId == recipientId && x.SeenOn != null)
            .Include(x => x.Notification)
            .Select(x => x.Notification)
        : context.NotificationRecipients
            .Where(x => x.RecipientId == recipientId)
            .Include(x => x.Notification)
            .Select(x => x.Notification);

    var count = await filtered.CountAsync();

    var items = await filtered
      .Skip((pageNumber - 1) * pageCount)
      .Take(pageCount)
      .ToListAsync();

    return items
      .Select(modelEntityConverter.ToModel)
      .OfType<NotificationModel>()
      .ToPaginatedList(count);
  }
}
