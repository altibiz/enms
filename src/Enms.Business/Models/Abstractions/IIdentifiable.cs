using System.ComponentModel.DataAnnotations;

namespace Enms.Business.Models.Abstractions;

public interface IIdentifiable : IValidatableObject, IModel
{
  string Title { get; }

  string Id { get; }
}
