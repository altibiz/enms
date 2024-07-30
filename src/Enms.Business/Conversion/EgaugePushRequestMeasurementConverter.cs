using System.Xml.Linq;
using Enms.Business.Conversion.Base;
using Enms.Business.Iot;
using Enms.Business.Models;

namespace Enms.Business.Conversion;

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
    XDocument request,
    DateTimeOffset timestamp)
  {
    return Parse(meterId, request);
  }

  protected override XDocument ToPushRequest(
    IEnumerable<EgaugeMeasurementModel> measurement)
  {
    throw new NotImplementedException();
  }

  private static IEnumerable<EgaugeMeasurementModel> Parse(
    string meterId,
    XDocument xml)
  {
    var result = new Dictionary<string, EgaugeRegister>();

    var group = xml.Descendants().First();
    var data = group.Descendants().First();
    var timestamp = DateTimeOffset.FromUnixTimeSeconds(
      Convert.ToInt64((string?)data.Attribute(XName.Get("time_stamp")), 16)
    );
    var registers = data.Descendants(XName.Get("cname"));
    var columns = data.Descendants(XName.Get("r")).First().Descendants();

    foreach (var (register, column) in registers.Zip(columns))
    {
      var registerName = (string)register;
      var type =
        ((string?)register.Attribute(XName.Get("t")))?.ToEgaugeRegisterType()
        ?? throw new ArgumentException(
          "Register {registerName} has no type",
          registerName
        );
      var value = (decimal)column;
      result[registerName] = new EgaugeRegister(type, type.Unit(), value);
    }

    var lines = result.Keys.Select(k => k.Split('-')[0]).Distinct();
    foreach (var line in lines)
    {
      yield return new EgaugeMeasurementModel
      {
        LineId = $"{meterId}-{line}",
        Timestamp = timestamp,
        VoltageL1AnyT0_V = result[line + "-VoltageL1AnyT0_V"].Value,
        VoltageL2AnyT0_V = result[line + "-VoltageL2AnyT0_V"].Value,
        VoltageL3AnyT0_V = result[line + "-VoltageL3AnyT0_V"].Value,
        CurrentL1AnyT0_A = result[line + "-CurrentL1AnyT0_A"].Value,
        CurrentL2AnyT0_A = result[line + "-CurrentL2AnyT0_A"].Value,
        CurrentL3AnyT0_A = result[line + "-CurrentL3AnyT0_A"].Value,
        ActivePowerL1NetT0_W = result[line + "-ActivePowerL1NetT0_W"].Value,
        ActivePowerL2NetT0_W = result[line + "-ActivePowerL2NetT0_W"].Value,
        ActivePowerL3NetT0_W = result[line + "-ActivePowerL3NetT0_W"].Value,
        ApparentPowerL1NetT0_W = result[line + "-ApparentPowerL1NetT0_W"].Value,
        ApparentPowerL2NetT0_W = result[line + "-ApparentPowerL2NetT0_W"].Value,
        ApparentPowerL3NetT0_W = result[line + "-ApparentPowerL3NetT0_W"].Value,
      };
    }
  }
}
