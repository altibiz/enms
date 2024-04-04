using Enms.Fake.Correction.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Correction.Agnostic;

public class AgnosticCumulativeCorrector(IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public IMeasurementRecord CorrectCumulatives(
    DateTimeOffset timestamp,
    IMeasurementRecord measurementRecord,
    IMeasurementRecord firstMeasurementRecord,
    IMeasurementRecord lastMeasurementRecord
  )
  {
    var corrector = _serviceProvider.GetServices<ICumulativeCorrector>()
        .FirstOrDefault(
          c =>
            c.CanCorrectCumulativesFor(
              measurementRecord.GetType()))
      ?? throw new InvalidOperationException(
        $"No corrector found for {measurementRecord.GetType().Name}");

    return corrector.CorrectCumulatives(
      timestamp,
      measurementRecord,
      firstMeasurementRecord,
      lastMeasurementRecord
    );
  }
}
