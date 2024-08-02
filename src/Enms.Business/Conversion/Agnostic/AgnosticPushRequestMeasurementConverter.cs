using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Agnostic;

public class AgnosticPushRequestMeasurementConverter(
  IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public Stream ToPushRequest(
    string meterId,
    IEnumerable<IMeasurement> measurement)
  {
    return _serviceProvider
        .GetServices<IPushRequestMeasurementConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvert(meterId))
        ?.ToPushRequest(measurement)
      ?? throw new InvalidOperationException(
        $"No converter found for measurement {measurement.GetType()}.");
  }

  public HttpContent ToHttpContent(
    string meterId,
    IEnumerable<IMeasurement> measurement)
  {
    return _serviceProvider
        .GetServices<IPushRequestMeasurementConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvert(meterId))
        ?.ToHttpContent(measurement)
      ?? throw new InvalidOperationException(
        $"No converter found for measurement {measurement.GetType()}.");
  }

  public IEnumerable<IMeasurement> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request)
  {
    return _serviceProvider
        .GetServices<IPushRequestMeasurementConverter>()
        .FirstOrDefault(converter => converter.CanConvert(meterId))
        ?.ToMeasurements(meterId, timestamp, request)
      ?? throw new InvalidOperationException(
        $"No converter found for meter {meterId}.");
  }
}
