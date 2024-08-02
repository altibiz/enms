using Enms.Business.Models.Abstractions;
using Enms.Fake.Correction.Abstractions;

namespace Enms.Fake.Correction.Base;

public abstract class
  MeasurementCorrector<TMeasurement> : IMeasurementCorrector
  where TMeasurement : class, IMeasurement
{
  public bool CanCorrectCumulativesFor(Type measurementRecordType)
  {
    return measurementRecordType.IsAssignableFrom(typeof(TMeasurement));
  }

  public IMeasurement Correct(
    DateTimeOffset timestamp,
    string meterId,
    string lineId,
    IMeasurement measurementRecord,
    IMeasurement firstMeasurementRecord,
    IMeasurement lastMeasurementRecord
  )
  {
    return CorrectCumulatives(
      timestamp,
      CopyCorrectId(
        measurementRecord as TMeasurement
        ?? throw new ArgumentException(
          $"Expected {
            typeof(TMeasurement).Name
          }, but got {
            measurementRecord.GetType().Name
          }",
          nameof(measurementRecord)
        ),
        meterId,
        lineId),
      firstMeasurementRecord
        as TMeasurement
      ?? throw new ArgumentException(
        $"Expected {
          typeof(TMeasurement).Name
        }, but got {
          firstMeasurementRecord.GetType().Name
        }",
        nameof(firstMeasurementRecord)
      ),
      lastMeasurementRecord
        as TMeasurement
      ?? throw new ArgumentException(
        $"Expected {
          typeof(TMeasurement).Name
        }, but got {
          lastMeasurementRecord.GetType().Name
        }",
        nameof(lastMeasurementRecord)
      )
    );
  }

  protected abstract TMeasurement CorrectCumulatives(
    DateTimeOffset timestamp,
    TMeasurement measurementRecord,
    TMeasurement firstMeasurementRecord,
    TMeasurement lastMeasurementRecord
  );

  protected abstract TMeasurement CopyCorrectId(
    TMeasurement record,
    string meterId,
    string lineId
  );

  protected decimal DiffMultiplier(
    DateTimeOffset timestamp,
    DateTimeOffset firstTimestamp,
    DateTimeOffset lastTimestamp
  )
  {
    var multiplier = (timestamp - firstTimestamp).Ticks /
      (lastTimestamp - firstTimestamp).Ticks;
    return multiplier;
  }
}
