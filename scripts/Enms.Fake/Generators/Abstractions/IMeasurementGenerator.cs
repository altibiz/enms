using Enms.Business.Iot;

namespace Enms.Fake.Generators.Abstractions;

public interface IMeasurementGenerator
{
  bool CanGenerateMeasurementsFor(string lineId);

  Task<List<MessengerPushRequestMeasurement>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string lineId,
    CancellationToken cancellationToken = default
  );
}
