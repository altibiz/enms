using System.Globalization;
using System.Text.Json;
using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Conversion;

// TODO: make this less repetitive
// TODO: per line register map
// FIXME: apparent power mess

public sealed class
  EgaugePushRequestMeasurementConverter : PushRequestMeasurementConverter<
  EgaugeMeasurementModel>
{
  private const string Separator = "@";

  private static readonly JsonSerializerOptions JsonSerializerOptions =
    new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

  private static readonly Dictionary<string, string> RegisterMap = new()
  {
    [nameof(EgaugeMeasurementModel.VoltageL1AnyT0_V)] = "VoltageL1AnyT0_V",
    [nameof(EgaugeMeasurementModel.VoltageL2AnyT0_V)] = "VoltageL2AnyT0_V",
    [nameof(EgaugeMeasurementModel.VoltageL3AnyT0_V)] = "VoltageL3AnyT0_V",
    [nameof(EgaugeMeasurementModel.CurrentL1AnyT0_A)] = "CurrentL1AnyT0_A",
    [nameof(EgaugeMeasurementModel.CurrentL2AnyT0_A)] = "CurrentL2AnyT0_A",
    [nameof(EgaugeMeasurementModel.CurrentL3AnyT0_A)] = "CurrentL3AnyT0_A",
    [nameof(EgaugeMeasurementModel.ActivePowerL1NetT0_W)] =
      "ActivePowerL1NetT0_W",
    [nameof(EgaugeMeasurementModel.ActivePowerL2NetT0_W)] =
      "ActivePowerL2NetT0_W",
    [nameof(EgaugeMeasurementModel.ActivePowerL3NetT0_W)] =
      "ActivePowerL3NetT0_W",
    [nameof(EgaugeMeasurementModel.ApparentPowerL1NetT0_W)] =
      "ApparentPowerL1NetT0_W",
    [nameof(EgaugeMeasurementModel.ApparentPowerL2NetT0_W)] =
      "ApparentPowerL2NetT0_W",
    [nameof(EgaugeMeasurementModel.ApparentPowerL3NetT0_W)] =
      "ApparentPowerL3NetT0_W"
  };

  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  public override async Task<IEnumerable<IMeasurement>> ToMeasurements(
    string meterId,
    DateTimeOffset timestamp,
    Stream request)
  {
    var parsed = await JsonSerializer.DeserializeAsync<EgaugePushRequest>(
      request, JsonSerializerOptions);

    var schema = parsed.Registers
      .Select(r => r.Name.Split(Separator)[1])
      .Where(x => !x.EndsWith('*'))
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
              x =>
                (x.Register.Name.Split(Separator)[1] == lineId &&
                  !x.Register.Name.StartsWith(
                    "ApparentPower",
                    StringComparison.InvariantCulture)) ||
                (x.Register.Name.Split(Separator)[1] == $"{lineId}*" &&
                  x.Register.Name.StartsWith(
                    "ApparentPower",
                    StringComparison.InvariantCulture)))
            .ToDictionary(
              x => x.Register.Name.Split(Separator)[0],
              x => x.Index)
        })
      .ToList();

    var measurements = parsed.Ranges.SelectMany(
      range =>
      {
        var timestamp = DateTimeOffset
          .FromUnixTimeSeconds(
            long.Parse(range.Ts, CultureInfo.InvariantCulture))
          .ToUniversalTime();
        var delta = TimeSpan.FromSeconds((double)range.Delta);

        // NOTE: for some reason, the first row is bonkers
        return range.Rows.Skip(1).SelectMany(
          (row, i) =>
          {
            var rowTimestamp = timestamp + delta * i;
            return schema
              .Select(
                line =>
                {
                  var voltageL1AnyT0_V =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.VoltageL1AnyT0_V))]];
                  var voltageL2AnyT0_V =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.VoltageL2AnyT0_V))]];
                  var voltageL3AnyT0_V =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.VoltageL3AnyT0_V))]];
                  var currentL1AnyT0_A =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.CurrentL1AnyT0_A))]];
                  var currentL2AnyT0_A =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.CurrentL2AnyT0_A))]];
                  var currentL3AnyT0_A =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.CurrentL3AnyT0_A))]];
                  var activePowerL1NetT0_W =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.ActivePowerL1NetT0_W))]];
                  var activePowerL2NetT0_W =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.ActivePowerL2NetT0_W))]];
                  var activePowerL3NetT0_W =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel.ActivePowerL3NetT0_W))]];
                  var apparentPowerL1NetT0_W =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel
                          .ApparentPowerL1NetT0_W))]];
                  var apparentPowerL2NetT0_W =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel
                          .ApparentPowerL2NetT0_W))]];
                  var apparentPowerL3NetT0_W =
                    row[line.Measures[
                      Register(
                        nameof(EgaugeMeasurementModel
                          .ApparentPowerL3NetT0_W))]];

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
                    VoltageL1AnyT0_V = -decimal.Parse(
                      voltageL1AnyT0_V, CultureInfo.InvariantCulture) / 1000M,
                    VoltageL2AnyT0_V = -decimal.Parse(
                      voltageL2AnyT0_V, CultureInfo.InvariantCulture) / 1000M,
                    VoltageL3AnyT0_V = -decimal.Parse(
                      voltageL3AnyT0_V, CultureInfo.InvariantCulture) / 1000M,
                    CurrentL1AnyT0_A = -decimal.Parse(
                      currentL1AnyT0_A, CultureInfo.InvariantCulture) / 1000M,
                    CurrentL2AnyT0_A = -decimal.Parse(
                      currentL2AnyT0_A, CultureInfo.InvariantCulture) / 1000M,
                    CurrentL3AnyT0_A = -decimal.Parse(
                      currentL3AnyT0_A, CultureInfo.InvariantCulture) / 1000M,
                    ActivePowerL1NetT0_W = -decimal.Parse(
                      activePowerL1NetT0_W, CultureInfo.InvariantCulture),
                    ActivePowerL2NetT0_W = -decimal.Parse(
                      activePowerL2NetT0_W, CultureInfo.InvariantCulture),
                    ActivePowerL3NetT0_W = -decimal.Parse(
                      activePowerL3NetT0_W, CultureInfo.InvariantCulture),
                    ApparentPowerL1NetT0_W =
                      -decimal.Parse(
                        apparentPowerL1NetT0_W, CultureInfo.InvariantCulture),
                    ApparentPowerL2NetT0_W =
                      -decimal.Parse(
                        apparentPowerL2NetT0_W, CultureInfo.InvariantCulture),
                    ApparentPowerL3NetT0_W =
                      -decimal.Parse(
                        apparentPowerL3NetT0_W, CultureInfo.InvariantCulture)
                  };
                })
              .Where(x => x is not null)
              .Cast<IMeasurement>();
          });
      });

    return measurements;
  }

  protected override HttpContent ToHttpContent(
    IEnumerable<EgaugeMeasurementModel> measurements)
  {
    var lineIds = measurements.Select(m => m.LineId).Distinct().ToList();

    var request = new EgaugePushRequest(
      lineIds
        .SelectMany<string, EgaugePushRequestRegister>(
          lineId =>
          [
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.VoltageL1AnyT0_V))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Voltage,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.VoltageL2AnyT0_V))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Voltage,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.VoltageL3AnyT0_V))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Voltage,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.CurrentL1AnyT0_A))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Current,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.CurrentL2AnyT0_A))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Current,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.CurrentL3AnyT0_A))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Current,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.ActivePowerL1NetT0_W))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Power,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.ActivePowerL2NetT0_W))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Power,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.ActivePowerL3NetT0_W))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.Power,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.ApparentPowerL1NetT0_W))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.ApparentPower,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.ApparentPowerL2NetT0_W))
              + Separator
              + lineId,
              EgaugeRegisterTypeString.ApparentPower,
              default
            ),
            new EgaugePushRequestRegister(
              Register(nameof(EgaugeMeasurementModel.ApparentPowerL3NetT0_W))
              + Separator
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

            registers[startingIndex + 0] =
              m.VoltageL1AnyT0_V.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 1] =
              m.VoltageL2AnyT0_V.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 2] =
              m.VoltageL3AnyT0_V.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 3] =
              m.CurrentL1AnyT0_A.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 4] =
              m.CurrentL2AnyT0_A.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 5] =
              m.CurrentL3AnyT0_A.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 6] =
              m.ActivePowerL1NetT0_W.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 7] =
              m.ActivePowerL2NetT0_W.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 8] =
              m.ActivePowerL3NetT0_W.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 9] =
              m.ApparentPowerL1NetT0_W.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 10] =
              m.ApparentPowerL2NetT0_W.ToString(CultureInfo.InvariantCulture);
            registers[startingIndex + 11] =
              m.ApparentPowerL3NetT0_W.ToString(CultureInfo.InvariantCulture);

            return new EgaugePushRequestRange(
              m.Timestamp
                .ToUniversalTime()
                .ToUnixTimeSeconds()
                .ToString(CultureInfo.InvariantCulture),
              default,
              [registers]
            );
          })
        .ToList()
    );

    var stream = new MemoryStream();
    JsonSerializer.Serialize(stream, request);

    return JsonContent.Create(request);
  }

  private static string Register(string measurement)
  {
    return RegisterMap[measurement];
  }
}
