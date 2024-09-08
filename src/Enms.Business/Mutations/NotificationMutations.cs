using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Models.Joins;
using Enms.Business.Mutations.Abstractions;
using Enms.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Mutations;

public class NotificationMutations(
  DataDbContext context,
  IServiceProvider serviceProvider
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

    var recipients = representatives
      .Select(x => new NotificationRecipientModel
      {
        NotificationId = model.Id,
        RepresentativeId = x.Id
      })
      .ToList();

    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(model.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {model.GetType()}");
    context.Add(modelEntityConverter.ToEntity(model));
    context.AddRange(recipients.Select(modelEntityConverter.ToEntity));

    await context.SaveChangesAsync();
  }
}
