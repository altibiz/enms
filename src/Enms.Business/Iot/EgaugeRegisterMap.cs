namespace Enms.Business.Iot;

public readonly record struct EgaugeRegisterMap(
  IDictionary<string, EgaugeRegister> Registers,
  string MeterId,
  DateTimeOffset Timestamp
)
{
  public float Power_W
  {
    get
    {
      return Registers.TryGetValue("P", out var register)
        ? (float)register.Value
        : default;
    }
  }

  public float Voltage_V
  {
    get
    {
      return Registers.TryGetValue("V1 Voltage", out var register)
        ? (float)register.Value
        : default;
    }
  }
}
