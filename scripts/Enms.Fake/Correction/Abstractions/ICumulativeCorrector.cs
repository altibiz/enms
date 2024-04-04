using Enms.Fake.Records.Abstractions;

namespace Enms.Fake.Correction.Abstractions;

public interface ICumulativeCorrector
{
  bool CanCorrectCumulativesFor(Type measurementRecordType);

  IMeasurementRecord CorrectCumulatives(
    DateTimeOffset timestamp,
    IMeasurementRecord measurementRecord,
    IMeasurementRecord firstMeasurementRecord,
    IMeasurementRecord lastMeasurementRecord
  );
}
