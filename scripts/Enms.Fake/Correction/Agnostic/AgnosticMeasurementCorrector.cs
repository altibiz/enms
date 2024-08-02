using Enms.Business.Models.Abstractions;
using Enms.Fake.Correction.Abstractions;

namespace Enms.Fake.Correction.Agnostic;

public class AgnosticCumulativeCorrector(IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public IMeasurement Correct(
    DateTimeOffset timestamp,
    string meterId,
    string lineId,
    IMeasurement measurementRecord,
    IMeasurement firstMeasurementRecord,
    IMeasurement lastMeasurementRecord
  )
  {
    var corrector = _serviceProvider.GetServices<IMeasurementCorrector>()
        .FirstOrDefault(
          c =>
            c.CanCorrectCumulativesFor(
              measurementRecord.GetType()))
      ?? throw new InvalidOperationException(
        $"No corrector found for {measurementRecord.GetType().Name}");

    return corrector.Correct(
      timestamp,
      meterId,
      lineId,
      measurementRecord,
      firstMeasurementRecord,
      lastMeasurementRecord
    );
  }
}
