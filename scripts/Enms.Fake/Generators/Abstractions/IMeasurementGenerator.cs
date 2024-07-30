using System.Xml.Linq;

namespace Enms.Fake.Generators.Abstractions;

public interface IMeasurementGenerator
{
  bool CanGenerateMeasurementsFor(string lineId);

  Task<List<XDocument>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string lineId,
    CancellationToken cancellationToken = default
  );
}
