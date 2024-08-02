using Enms.Business.Models.Abstractions;
using Enms.Fake.Conversion.Abstractions;
using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Conversion.Base;

public abstract class
  MeasurementRecordMeasurementConverter<TRecord> :
  IMeasurementRecordMeasurementConverter
  where TRecord : IMeasurementRecord
{
  protected abstract string MeterIdPrefix { get; }

  public bool CanConvertToMeasurement(string meterId)
  {
    return meterId.StartsWith(MeterIdPrefix);
  }

  public IMeasurement ConvertToMeasurement(
    IMeasurementRecord record)
  {
    return ConvertToMeasurementConcrete(
      record is TRecord concreteRecord
        ? concreteRecord
        : throw new InvalidOperationException(
          $"Record is not of type {typeof(TRecord).Name}."));
  }

  protected abstract IMeasurement ConvertToMeasurementConcrete(TRecord record);
}
