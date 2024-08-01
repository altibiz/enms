using System.Text.Json.Nodes;
using Enms.Fake.Generators.Abstractions;

namespace Enms.Fake.Generators.Agnostic;

public class AgnosticMeasurementGenerator(IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public async Task<List<JsonNode>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId,
    CancellationToken cancellationToken = default
  )
  {
    var logger = _serviceProvider
      .GetRequiredService<ILogger<AgnosticMeasurementGenerator>>();
    var generators = _serviceProvider.GetServices<IMeasurementGenerator>();
    var generator =
      generators.FirstOrDefault(g => g.CanGenerateMeasurementsFor(meterId));
    var measurements =
      await (generator?.GenerateMeasurements(
          dateFrom, dateTo, meterId, cancellationToken)
        ?? throw new InvalidOperationException(
          $"No generator found for line {meterId}"));
    logger.LogInformation(
      "Generated {Count} measurements for line {LineId} from {DateFrom} to {DateTo}",
      measurements.Count,
      meterId,
      dateFrom,
      dateTo
    );
    return measurements;
  }
}
