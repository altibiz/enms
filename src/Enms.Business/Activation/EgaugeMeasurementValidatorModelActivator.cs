using Enms.Business.Activation.Base;
using Enms.Business.Models;

namespace Enms.Business.Activation;

public class
  EgaugeMeasurementValidatorModelActivator : ModelActivator<
  EgaugeMeasurementValidatorModel>
{
  public override EgaugeMeasurementValidatorModel ActivateConcrete()
  {
    return new EgaugeMeasurementValidatorModel
    {
      Id = default!,
      Title = "",
      CreatedOn = DateTimeOffset.UtcNow,
      CreatedById = default!,
      LastUpdatedOn = default!,
      LastUpdatedById = default!,
      IsDeleted = false,
      DeletedOn = null,
      DeletedById = null
    };
  }
}
