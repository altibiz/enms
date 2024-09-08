using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Models.Joins;
using Enms.Business.Mutations.Abstractions;
using Enms.Data.Context;
using Enms.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Mutations;

// TODO: paging when fetching

public class NotificationMutations(
  DataDbContext context,
  AgnosticModelEntityConverter modelEntityConverter
) : IMutations
{
  public async Task Create(INotification model)
  {
    var validationResults = model
      .Validate(new ValidationContext(this))
      .ToList();
    if (validationResults.Count is not 0)
    {
      throw new ValidationException(validationResults.First().ErrorMessage);
    }

    var topics = model.Topics.Select(x => x.ToEntity()).ToList();

    var representatives = await context.Representatives
      .Where(x => x.Topics.Any(y => topics.Contains(y)))
      .ToListAsync();

    var entity = modelEntityConverter.ToEntity<NotificationEntity>(model);
    context.Add(entity);
    await context.SaveChangesAsync();

    var recipients = representatives
      .Select(x => new NotificationRecipientModel
      {
        NotificationId = entity.Id,
        RepresentativeId = x.Id
      })
      .ToList();
    context.AddRange(recipients.Select(modelEntityConverter.ToEntity));
    await context.SaveChangesAsync();
  }

  public async Task Seen(INotification model, RepresentativeModel representative)
  {
    var recipient = await context.NotificationRecipients
      .FirstOrDefaultAsync(x => x.NotificationId == model.Id &&
                                x.RecipientId == representative.Id);

    if (recipient is null)
    {
      throw new InvalidOperationException(
        $"No notification recipient found for {model.Id} "
        + $"and {representative.Id}");
    }

    recipient.SeenOn = DateTimeOffset.UtcNow;
    context.Update(recipient);
    await context.SaveChangesAsync();
  }
}
