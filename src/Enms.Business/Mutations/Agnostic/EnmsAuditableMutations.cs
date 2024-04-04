using System.ComponentModel.DataAnnotations;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;
using Enms.Business.Mutations.Abstractions;
using Enms.Data;
using Microsoft.EntityFrameworkCore;

// TODO: check representative model user id

namespace Enms.Business.Mutations.Agnostic;

public class EnmsAuditableMutations : IEnmsMutations
{
  private readonly EnmsDbContext _context;

  private readonly IServiceProvider _serviceProvider;

  public EnmsAuditableMutations(
    EnmsDbContext context,
    IServiceProvider serviceProvider
  )
  {
    _context = context;
    _serviceProvider = serviceProvider;
  }

  public async ValueTask DisposeAsync()
  {
    await _context.SaveChangesAsync();
    GC.SuppressFinalize(this);
  }

  public void Dispose()
  {
    _context.SaveChanges();
    GC.SuppressFinalize(this);
  }

  public void ClearChanges()
  {
    _context.ChangeTracker.Clear();
  }

  public void Create(IAuditable auditable)
  {
    var validationResults = auditable
      .Validate(new ValidationContext(this))
      .ToList();
    if (validationResults.Count is not 0)
    {
      throw new ValidationException(validationResults.First().ErrorMessage);
    }

    var modelEntityConverter = _serviceProvider
                                 .GetServices<IModelEntityConverter>()
                                 .FirstOrDefault(converter => converter
                                   .CanConvertToEntity(auditable.GetType())) ??
                               throw new InvalidOperationException(
                                 $"No model entity converter found for {auditable.GetType()}");
    _context.Entry(modelEntityConverter.ToEntity(auditable)).State =
      EntityState.Added;
  }

  public void Update(IAuditable auditable)
  {
    var validationResults = auditable
      .Validate(new ValidationContext(this))
      .ToList();
    if (validationResults.Count is not 0)
    {
      throw new ValidationException(validationResults.First().ErrorMessage);
    }

    var modelEntityConverter = _serviceProvider
                                 .GetServices<IModelEntityConverter>()
                                 .FirstOrDefault(converter => converter
                                   .CanConvertToEntity(auditable.GetType())) ??
                               throw new InvalidOperationException(
                                 $"No model entity converter found for {auditable.GetType()}");
    _context.Update(modelEntityConverter.ToEntity(auditable));
  }

  public void Delete(IAuditable auditable)
  {
    var modelEntityConverter = _serviceProvider
                                 .GetServices<IModelEntityConverter>()
                                 .FirstOrDefault(converter => converter
                                   .CanConvertToEntity(auditable.GetType())) ??
                               throw new InvalidOperationException(
                                 $"No model entity converter found for {auditable.GetType()}");
    _context.Remove(modelEntityConverter.ToEntity(auditable));
  }
}
