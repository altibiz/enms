using System.Xml.Linq;
using Enms.Business.Conversion.Abstractions;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion.Agnostic;

public class AgnosticPushRequestMeasurementConverter(
  IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public XDocument ToPushRequest(IMeasurement measurement)
  {
    return _serviceProvider
        .GetServices<IPushRequestMeasurementConverter>()
        .FirstOrDefault(
          converter =>
            converter.CanConvert(measurement.MeterId))
        ?.ToPushRequest(measurement)
      ?? throw new InvalidOperationException(
        $"No converter found for measurement {measurement.GetType()}.");
  }

  public IMeasurement ToMeasurement(
    XDocument request,
    string meterId,
    DateTimeOffset timestamp)
  {
    return _serviceProvider
        .GetServices<IPushRequestMeasurementConverter>()
        .FirstOrDefault(converter => converter.CanConvert(meterId))
        ?.ToMeasurement(request, meterId, timestamp)
      ?? throw new InvalidOperationException(
        $"No converter found for meter {meterId}.");
  }

  public TMeasurement ToMeasurement<TMeasurement>(
    XDocument request,
    string meterId,
    DateTimeOffset timestamp)
    where TMeasurement : class, IMeasurement
  {
    return ToMeasurement(request, meterId, timestamp) as TMeasurement
      ?? throw new InvalidOperationException(
        $"No converter found for meter {meterId}.");
  }
}
