using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Data;

namespace Enms.Business.Mutations.Agnostic;

public class EnmsEventMutations : IEnmsMutations
{
  private readonly EnmsDataDbContext _context;

  private readonly IServiceProvider _serviceProvider;

  public EnmsEventMutations(
    EnmsDataDbContext context,
    IServiceProvider serviceProvider
  )
  {
    _context = context;
    _serviceProvider = serviceProvider;
  }

  public void ClearChanges()
  {
    _context.ChangeTracker.Clear();
  }

  public void Create(IEvent @event)
  {
    var validationResults = @event
      .Validate(new ValidationContext(this))
      .ToList();
    if (validationResults.Count is not 0)
    {
      throw new ValidationException(validationResults.First().ErrorMessage);
    }

    var modelEntityConverter = _serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(@event.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {@event.GetType()}");
    _context.Add(modelEntityConverter.ToEntity(@event));
  }
}
