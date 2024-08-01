using System.Text.Json.Nodes;

namespace Enms.Fake.Generators.Abstractions;

public interface IMeasurementGenerator
{
  bool CanGenerateMeasurementsFor(string meterId);

  Task<List<JsonNode>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId,
    CancellationToken cancellationToken = default
  );
}
