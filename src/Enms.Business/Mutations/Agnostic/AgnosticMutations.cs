using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Data;
using Microsoft.EntityFrameworkCore;

// TODO: check representative model user id

namespace Enms.Business.Mutations.Agnostic;

public class AgnosticMutations(
  EnmsDataDbContext context,
  IServiceProvider serviceProvider
  ) : IMutations
{
  public Task Save()
  {
    return context.SaveChangesAsync();
  }

  public void Create(IModel model)
  {
    if (model is IValidatableObject validatable)
    {
      var validationResults = validatable
        .Validate(new ValidationContext(this))
        .ToList();
      if (validationResults.Count is not 0)
      {
        throw new ValidationException(validationResults.First().ErrorMessage);
      }
    }

    var modelEntityConverter = serviceProvider
        .GetServices<IModelEntityConverter>()
        .FirstOrDefault(
          converter => converter
            .CanConvertToEntity(model.GetType())) ??
      throw new InvalidOperationException(
        $"No model entity converter found for {model.GetType()}");
    context.Entry(modelEntityConverter.ToEntity(model)).State =
      EntityState.Added;
  }

  public void Update(IModel model)
  {
    if (model is IValidatableObject validatable)
    {
      var validationResults = validatable
        .Validate(new ValidationContext(this))
        .ToList();
      if (validationResults.Count is not 0)
      {
        throw new ValidationException(validationResults.First().ErrorMessage);
      }
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

  public void Delete(IModel model)
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
