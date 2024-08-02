using Enms.Business.Models.Abstractions;

namespace Enms.Fake.Generators.Abstractions;

public interface IMeasurementGenerator
{
  bool CanGenerateMeasurementsFor(string meterId);

  Task<List<IMeasurement>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId,
    string lineId,
    CancellationToken cancellationToken = default
  );
}
