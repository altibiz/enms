using Enms.Data.Entities.Base;

namespace Enms.Data.Entities;

public class EgaugeMeterEntity : MeterEntity<
  EgaugeMeasurementEntity,
  EgaugeAggregateEntity,
  EgaugeMeasurementValidatorEntity>
{
}
