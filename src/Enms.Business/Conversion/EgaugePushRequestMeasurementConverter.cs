using System.Text.Json;
using Enms.Business.Conversion.Base;
using Enms.Business.Iot;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Data;

namespace Enms.Business.Conversion;

// TODO: make this less repetitive

public sealed class
  EgaugePushRequestMeasurementConverter : PushRequestMeasurementConverter<
  EgaugeMeasurementModel>
{
  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  public override IEnumerable<EgaugeMeasurementModel> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request)
  {
    var parsed = JsonSerializer.Deserialize<EgaugePushRequest>(request);

    var schema = parsed.Registers
      .Select(r => r.Name.Split(EnmsDataDbContext.KeyJoin)[1])
      .Distinct()
      .Select(
        lineId => new
        {
          LineId = lineId,
          Measures = parsed.Registers
            .Select(
              (r, i) => new
              {
                Index = i,
                Register = r
              })
            .Where(
              x => x.Register.Name.Split(EnmsDataDbContext.KeyJoin)[1]
                == lineId)
            .ToDictionary(
              x => x.Register.Name.Split(EnmsDataDbContext.KeyJoin)[0],
              x => x.Index)
        })
      .ToList();

    var measurements = parsed.Ranges.SelectMany(
      range =>
      {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(
          long.Parse(range.Ts));
        var delta = TimeSpan.FromSeconds((double)range.Delta);

        return range.Rows.SelectMany(
          (row, i) =>
          {
            var rowTimestamp = timestamp + delta * i;
            return schema
              .Select(
                line =>
                {
                  var voltageL1AnyT0_V =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.VoltageL1AnyT0_V)]];
                  var voltageL2AnyT0_V =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.VoltageL2AnyT0_V)]];
                  var voltageL3AnyT0_V =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.VoltageL3AnyT0_V)]];
                  var currentL1AnyT0_A =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.CurrentL1AnyT0_A)]];
                  var currentL2AnyT0_A =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.CurrentL2AnyT0_A)]];
                  var currentL3AnyT0_A =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.CurrentL3AnyT0_A)]];
                  var activePowerL1NetT0_W =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.ActivePowerL1NetT0_W)]];
                  var activePowerL2NetT0_W =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.ActivePowerL2NetT0_W)]];
                  var activePowerL3NetT0_W =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.ActivePowerL3NetT0_W)]];
                  var apparentPowerL1NetT0_W =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.ApparentPowerL1NetT0_W)]];
                  var apparentPowerL2NetT0_W =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.ApparentPowerL2NetT0_W)]];
                  var apparentPowerL3NetT0_W =
                    row[line.Measures[
                      nameof(EgaugeMeasurementModel.ApparentPowerL3NetT0_W)]];

                  if (string.IsNullOrEmpty(voltageL1AnyT0_V) &&
                    string.IsNullOrEmpty(voltageL2AnyT0_V) &&
                    string.IsNullOrEmpty(voltageL3AnyT0_V) &&
                    string.IsNullOrEmpty(currentL1AnyT0_A) &&
                    string.IsNullOrEmpty(currentL2AnyT0_A) &&
                    string.IsNullOrEmpty(currentL3AnyT0_A) &&
                    string.IsNullOrEmpty(activePowerL1NetT0_W) &&
                    string.IsNullOrEmpty(activePowerL2NetT0_W) &&
                    string.IsNullOrEmpty(activePowerL3NetT0_W) &&
                    string.IsNullOrEmpty(apparentPowerL1NetT0_W) &&
                    string.IsNullOrEmpty(apparentPowerL2NetT0_W) &&
                    string.IsNullOrEmpty(apparentPowerL3NetT0_W))
                  {
                    return null;
                  }

                  return new EgaugeMeasurementModel
                  {
                    MeterId = meterId,
                    LineId = line.LineId,
                    Timestamp = rowTimestamp,
                    VoltageL1AnyT0_V = decimal.Parse(voltageL1AnyT0_V),
                    VoltageL2AnyT0_V = decimal.Parse(voltageL2AnyT0_V),
                    VoltageL3AnyT0_V = decimal.Parse(voltageL3AnyT0_V),
                    CurrentL1AnyT0_A = decimal.Parse(currentL1AnyT0_A),
                    CurrentL2AnyT0_A = decimal.Parse(currentL2AnyT0_A),
                    CurrentL3AnyT0_A = decimal.Parse(currentL3AnyT0_A),
                    ActivePowerL1NetT0_W = decimal.Parse(activePowerL1NetT0_W),
                    ActivePowerL2NetT0_W = decimal.Parse(activePowerL2NetT0_W),
                    ActivePowerL3NetT0_W = decimal.Parse(activePowerL3NetT0_W),
                    ApparentPowerL1NetT0_W =
                      decimal.Parse(apparentPowerL1NetT0_W),
                    ApparentPowerL2NetT0_W =
                      decimal.Parse(apparentPowerL2NetT0_W),
                    ApparentPowerL3NetT0_W =
                      decimal.Parse(apparentPowerL3NetT0_W)
                  };
                })
              .Where(x => x is not null)
              .Cast<EgaugeMeasurementModel>();
          });
      });

    return measurements;
  }

  protected override Stream ToPushRequest(
    IEnumerable<EgaugeMeasurementModel> measurements)
  {
    var lineIds = measurements.Select(m => m.LineId).Distinct().ToList();

    var request = new EgaugePushRequest(
      lineIds
        .SelectMany<string, EgaugeRegister>(
          lineId =>
          [
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.VoltageL1AnyT0_V)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Voltage,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.VoltageL2AnyT0_V)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Voltage,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.VoltageL3AnyT0_V)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Voltage,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.CurrentL1AnyT0_A)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Current,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.CurrentL2AnyT0_A)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Current,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.CurrentL3AnyT0_A)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Current,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.ActivePowerL1NetT0_W)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Power,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.ActivePowerL2NetT0_W)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Power,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.ActivePowerL3NetT0_W)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.Power,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.ApparentPowerL1NetT0_W)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.ApparentPower,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.ApparentPowerL2NetT0_W)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.ApparentPower,
              default
            ),
            new EgaugeRegister(
              nameof(EgaugeMeasurementModel.ApparentPowerL3NetT0_W)
              + EnmsDataDbContext.KeyJoin
              + lineId,
              EgaugeRegisterTypeString.ApparentPower,
              default
            )
          ])
        .ToList(),
      measurements
        .Select(
          m =>
          {
            var registers = Enumerable
              .Range(0, lineIds.Count * 12)
              .Select(_ => "")
              .ToList();

            var startingIndex = lineIds.IndexOf(m.LineId) * 12;

            registers[startingIndex + 0] = m.VoltageL1AnyT0_V.ToString();
            registers[startingIndex + 1] = m.VoltageL2AnyT0_V.ToString();
            registers[startingIndex + 2] = m.VoltageL3AnyT0_V.ToString();
            registers[startingIndex + 3] = m.CurrentL1AnyT0_A.ToString();
            registers[startingIndex + 4] = m.CurrentL2AnyT0_A.ToString();
            registers[startingIndex + 5] = m.CurrentL3AnyT0_A.ToString();
            registers[startingIndex + 6] = m.ActivePowerL1NetT0_W.ToString();
            registers[startingIndex + 7] = m.ActivePowerL2NetT0_W.ToString();
            registers[startingIndex + 8] = m.ActivePowerL3NetT0_W.ToString();
            registers[startingIndex + 9] = m.ApparentPowerL1NetT0_W.ToString();
            registers[startingIndex + 10] = m.ApparentPowerL2NetT0_W.ToString();
            registers[startingIndex + 11] = m.ApparentPowerL3NetT0_W.ToString();

            return new EgaugeRange(
              m.Timestamp.ToUnixTimeSeconds().ToString(),
              default,
              [registers]
            );
          })
        .ToList()
    );

    var stream = new MemoryStream();
    JsonSerializer.Serialize(stream, request);

    return stream;
  }

  public override HttpContent ToHttpContent(
    IEnumerable<IMeasurement> measurement)
  {
    return JsonContent.Create(ToPushRequest(measurement));
  }
}
