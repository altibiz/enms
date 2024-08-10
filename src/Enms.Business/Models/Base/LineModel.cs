using System.ComponentModel.DataAnnotations;
using Enms.Business.Capabilities.Abstractions;
using Enms.Business.Capabilities.Base;
using Enms.Business.Models.Abstractions;
using Enms.Data;

namespace Enms.Business.Models.Base;

public class LineModel : AuditableModel, ILine
{
  [Required]
  public required float ConnectionPower_W { get; set; }

  [Required]
  public required HashSet<PhaseModel> Phases { get; set; } = new();

  [Required]
  public required string LineId { get; set; }

  [Required]
  public required string MeasurementValidatorId { get; set; }

  public override required string Id
  {
    get { return $"{LineId}{EnmsDataDbContext.KeyJoin}{MeterId}"; }
    set
    {
      if (value is null)
      {
        LineId = default!;
        MeterId = default!;
        return;
      }

      var parts = value.Split(EnmsDataDbContext.KeyJoin);
      LineId = parts[0];
      MeterId = parts[1];
    }
  }

  [Required]
  public required string MeterId { get; set; }

  public virtual ILineCapabilities Capabilities
  {
    get { return new LineCapabilities(); }
  }

  public override IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext)
  {
    foreach (var validationResult in base.Validate(validationContext))
    {
      yield return validationResult;
    }

    if (ConnectionPower_W <= 0)
    {
      yield return new ValidationResult(
        "Connection power must be greater than 0",
        new[] { nameof(ConnectionPower_W) });
    }

    if (Phases.Count == 0)
    {
      yield return new ValidationResult(
        "At least one phase must be set",
        new[] { nameof(Phases) });
    }

    if (Phases.Count > 3)
    {
      yield return new ValidationResult(
        "Maximum of three phases can be set",
        new[] { nameof(Phases) });
    }

    if (Phases.Count != Phases.Distinct().Count())
    {
      yield return new ValidationResult(
        "Phases must be unique",
        new[] { nameof(Phases) });
    }
  }
}
