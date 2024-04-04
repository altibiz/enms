using Enms.Business.Conversion.Abstractions;
using Enms.Business.Mutations.Agnostic;

namespace Enms.Business.Iot;

public class EnmsIotHandler
{
  private readonly EnmsMeasurementMutations _measurementMutations;

  private readonly IServiceProvider _serviceProvider;

  public EnmsIotHandler(
    IServiceProvider serviceProvider,
    EnmsMeasurementMutations measurementMutations
  )
  {
    _serviceProvider = serviceProvider;
    _measurementMutations = measurementMutations;
  }

  public Task OnPush(string meterId, string request)
  {
    var pushRequestMeasurmentConverter = _serviceProvider
                                           .GetServices<
                                             IPushRequestMeasurementConverter>()
                                           .FirstOrDefault(converter =>
                                             converter.CanConvert(meterId)) ??
                                         throw new InvalidOperationException(
                                           $"No converter found for meter id '{meterId}'");
    var measurement = pushRequestMeasurmentConverter.ToMeasurement(
      request,
      meterId,
      DateTimeOffset.UtcNow
    );

    _measurementMutations.Create(measurement);

    return Task.CompletedTask;
  }
}
