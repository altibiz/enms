using System.ComponentModel.DataAnnotations;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models;

public class UserModel : IModel
{
  [Required]
  public required string Id { get; set; }

  [Required]
  public required string UserName { get; set; }

  [Required]
  public required string Email { get; set; }
}
