using System.Xml.Linq;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Base;

public abstract class
  MeasurementRecordPushRequestConverter<TRecord> :
  IMeasurementRecordPushRequestConverter
  where TRecord : IMeasurementRecord
{
  protected abstract string MeterIdPrefix { get; }

  public bool CanConvertToPushRequest(string meterId)
  {
    return meterId.StartsWith(MeterIdPrefix);
  }

  public XDocument ConvertToPushRequest(
    string meterId,
    IEnumerable<IMeasurementRecord> records)
  {
    return ConvertToPushRequestConcrete(meterId, records.Cast<TRecord>());
  }

  protected abstract XDocument ConvertToPushRequestConcrete(
    string meterId,
    IEnumerable<TRecord> record);
}
