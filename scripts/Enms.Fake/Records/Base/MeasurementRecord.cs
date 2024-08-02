using CsvHelper.Configuration.Attributes;
using Enms.Business.Math;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Records.Base;

public abstract class MeasurementRecord : IMeasurementRecord
{
  public required string MeterId { get; set; }

  public required string LineId { get; set; }

  public required DateTimeOffset Timestamp { get; set; }

  [Ignore]
  public abstract TariffMeasure<decimal> ActiveEnergy_Wh { get; }

  [Ignore]
  public abstract TariffMeasure<decimal> ReactiveEnergy_VARh { get; }

  [Ignore]
  public abstract TariffMeasure<decimal> ApparentEnergy_VAh { get; }
}
