using System.Xml.Linq;
using Enms.Business.Conversion.Base;
using Enms.Business.Iot;
using Enms.Business.Models;

namespace Enms.Business.Conversion;

public class
  EgaugePushRequestMeasurementConverter : PushRequestMeasurementConverter<
  EgaugeMeasurementModel>
{
  protected override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  public override IEnumerable<EgaugeMeasurementModel> ToMeasurements(
    XDocument request,
    DateTimeOffset timestamp)
  {
    return Parse(request);
  }

  protected override XDocument ToPushRequest(
    IEnumerable<EgaugeMeasurementModel> measurement)
  {
    throw new NotImplementedException();
  }

  private static IEnumerable<EgaugeMeasurementModel> Parse(XDocument xml)
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
        LineId = measurementRegisters.LineId,
        Timestamp = measurementRegisters.Timestamp,
        Voltage_V = measurementRegisters.Voltage_V,
        Power_W = measurementRegisters.Power_W
      };
    }
  }
}
