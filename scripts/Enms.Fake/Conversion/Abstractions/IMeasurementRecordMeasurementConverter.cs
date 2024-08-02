using Enms.Business.Models.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Abstractions;

public interface IMeasurementRecordMeasurementConverter
{
  public bool CanConvertToMeasurement(string meterId);

  public IMeasurement ConvertToMeasurement(IMeasurementRecord record);
}
