using Enms.Fake.Correction.Base;
using Enms.Fake.Records;

namespace Enms.Fake.Correction;

public class EgaugeMeasurementRecordCumulativeCorrector
  : CumulativeCorrector<EgaugeMeasurementRecord>
{
  protected override EgaugeMeasurementRecord CorrectCumulatives(
    DateTimeOffset timestamp,
    EgaugeMeasurementRecord measurementRecord,
    EgaugeMeasurementRecord firstMeasurementRecord,
    EgaugeMeasurementRecord lastMeasurementRecord
  )
  {
#pragma warning disable S125
    // var diffMultiplier = DiffMultiplier(
    //   timestamp,
    //   firstMeasurementRecord.Timestamp,
    //   lastMeasurementRecord.Timestamp
    // );

    // var activeEnergy = measurementRecord.ActiveEnergy_Wh
    //   .Add(
    //     lastMeasurementRecord.ActiveEnergy_Wh
    //       .Subtract(firstMeasurementRecord.ActiveEnergy_Wh)
    //       .Multiply(diffMultiplier)
    //   );

    // var reactiveEnergy = measurementRecord.ReactiveEnergy_VARh
    //   .Add(
    //     lastMeasurementRecord.ReactiveEnergy_VARh
    //       .Subtract(firstMeasurementRecord.ReactiveEnergy_VARh)
    //       .Multiply(diffMultiplier)
    //   );
#pragma warning restore S125

    return measurementRecord;
  }

  protected override EgaugeMeasurementRecord CopyRecord(
    EgaugeMeasurementRecord record)
  {
    return new EgaugeMeasurementRecord
    {
      MeterId = record.MeterId,
      Timestamp = record.Timestamp
    };
  }
}
