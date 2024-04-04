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

  public override EgaugeMeasurementModel ToMeasurement(
    XDocument request,
    string meterId,
    DateTimeOffset timestamp)
  {
    return Parse(request);
  }

  protected override XDocument ToPushRequest(EgaugeMeasurementModel measurement)
  {
    throw new NotImplementedException();
  }

  private static EgaugeMeasurementModel Parse(XDocument xml)
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

    var measurementRegisters = new EgaugeRegisterMap(
      result,
      "egauge",
      timestamp
    );

    var measurement = new EgaugeMeasurementModel
    {
      MeterId = measurementRegisters.MeterId,
      Timestamp = measurementRegisters.Timestamp,
      Voltage_V = measurementRegisters.Voltage_V,
      Power_W = measurementRegisters.Power_W
    };

    return measurement;
  }
}
