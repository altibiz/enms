namespace Enms.Fake.Generators.Abstractions;

public interface IMeasurementGenerator
{
  bool CanGenerateMeasurementsFor(string meterId);

  Task<List<string>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId
  );
}
