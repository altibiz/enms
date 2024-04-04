using Enms.Data.Entities;
using Enms.Fake.Generators.Base;

namespace Enms.Fake.Generators;

public class EgaugeRepeatingCsvResourceMeasurementGenerator :
  RepeatingCsvResourceMeasurementGenerator<EgaugeMeasurementEntity>
{
  public EgaugeRepeatingCsvResourceMeasurementGenerator(
    IServiceProvider serviceProvider)
    : base(serviceProvider)
  {
  }

  protected override string CsvResourceName
  {
    get { return "abb-B2x-measurements.csv"; }
  }

  protected override string MeterIdPrefix
  {
    get { return "abb-B2x"; }
  }
}
