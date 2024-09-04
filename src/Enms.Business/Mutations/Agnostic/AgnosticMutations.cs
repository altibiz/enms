using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Data.Context;

// TODO: check representative model user id

namespace Enms.Business.Mutations.Agnostic;

public class AgnosticMutations(
  DataDbContext context,
  IServiceProvider serviceProvider
) : IMutations
{
  public Task Save()
  {
    return context.SaveChangesAsync();
  }

  public void Create(IAuditable model)
  {
    var validationResults = model
      .Validate(new ValidationContext(this))
      .ToList();
    if (validationResults.Count is not 0)
    {
      throw new ValidationException(validationResults.First().ErrorMessage);
    }

    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(model.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {model.GetType()}");
    context.Add(modelEntityConverter.ToEntity(model));
  }

  public void Update(IAuditable model)
  {
    var validationResults = model
      .Validate(new ValidationContext(this))
      .ToList();
    if (validationResults.Count is not 0)
    {
      throw new ValidationException(validationResults.First().ErrorMessage);
    }

    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(model.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {model.GetType()}");
    context.Update(modelEntityConverter.ToEntity(model));
  }

  public void Delete(IAuditable model)
  {
    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(model.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {model.GetType()}");
    context.Remove(modelEntityConverter.ToEntity(model));
  }
}
