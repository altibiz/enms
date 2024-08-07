using Enms.Business.Models.Abstractions;
using Enms.Fake.Generators.Abstractions;

namespace Enms.Fake.Generators.Agnostic;

public class AgnosticMeasurementGenerator(IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public async Task<List<IMeasurement>> GenerateMeasurements(
    DateTimeOffset dateFrom,
    DateTimeOffset dateTo,
    string meterId,
    string lineId,
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
          dateFrom, dateTo, meterId, lineId, cancellationToken)
        ?? throw new InvalidOperationException(
          $"No generator found for line {meterId}"));
    logger.LogInformation(
      "Generated {Count} measurements for line {LineId}@{MeterId} from {DateFrom} to {DateTo}",
      measurements.Count,
      lineId,
      meterId,
      dateFrom,
      dateTo
    );
    return measurements;
  }
}
