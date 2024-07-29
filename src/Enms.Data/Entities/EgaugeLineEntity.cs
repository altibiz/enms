using Enms.Data.Entities.Base;

namespace Enms.Data.Entities;

public class EgaugeLineEntity : LineEntity<
  EgaugeMeasurementEntity,
  EgaugeAggregateEntity,
  EgaugeMeasurementValidatorEntity>
{
}
