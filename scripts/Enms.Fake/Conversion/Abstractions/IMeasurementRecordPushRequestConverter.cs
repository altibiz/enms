using System.Xml.Linq;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Abstractions;

public interface IMeasurementRecordPushRequestConverter
{
  public bool CanConvertToPushRequest(string meterId);

  public XDocument ConvertToPushRequest(
    string meterId,
    IEnumerable<IMeasurementRecord> records);
}
