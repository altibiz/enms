using Enms.Fake.Generators.Base;
using Enms.Fake.Records;

namespace Enms.Fake.Generators;

public class EgaugeRepeatingCsvResourceMeasurementGenerator(
  IServiceProvider serviceProvider) :
  RepeatingCsvResourceMeasurementGenerator<EgaugeMeasurementRecord>(
    serviceProvider)
{
  protected override string CsvResourceName
  {
    get { return "egauge-measurements.csv"; }
  }

  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }
}
