using Enms.Business.Models.Abstractions;

namespace Enms.Fake.Correction.Abstractions;

public interface IMeasurementCorrector
{
  bool CanCorrectCumulativesFor(Type measurementRecordType);

  IMeasurement Correct(
    DateTimeOffset timestamp,
    string meterId,
    string lineId,
    IMeasurement measurementRecord,
    IMeasurement firstMeasurementRecord,
    IMeasurement lastMeasurementRecord
  );
}
