using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Data;

namespace Enms.Business.Mutations.Agnostic;

public class EnmsMeasurementMutations : IEnmsMutations
{
  private readonly EnmsDataDbContext _context;

  private readonly IServiceProvider _serviceProvider;

  public EnmsMeasurementMutations(
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

  public void Create(IMeasurement measurement)
  {
    var validationResults = measurement
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
            .CanConvertToEntity(
              measurement.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {measurement.GetType()}");
    _context.Add(modelEntityConverter.ToEntity(measurement));
  }
}
