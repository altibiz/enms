using System.Xml.Linq;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Agnostic;

public class AgnosticMeasurementRecordPushRequestConverter(
  IServiceProvider serviceProvider)
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public XDocument ConvertToPushRequest(
    string meterId,
    IEnumerable<IMeasurementRecord> records)
  {
    var converter = _serviceProvider
      .GetServices<IMeasurementRecordPushRequestConverter>()
      .FirstOrDefault(c => c.CanConvertToPushRequest(meterId));

    return converter?.ConvertToPushRequest(meterId, records)
      ?? throw new InvalidOperationException(
        $"No converter found for {records.GetType()}");
  }
}
