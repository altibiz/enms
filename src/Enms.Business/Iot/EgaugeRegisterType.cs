namespace Enms.Business.Iot;

public enum EgaugeRegisterType
{
  Irradiance,
  Frequency,
  Current,
  ReactivePower,
  Pressure,
  Power,
  VolumetricFlow,
  MassFlow,
  Resistance,
  ApparentPower,
  TotalHarmonicDistortion,
  Temperature,
  Voltage,
  Numeric,
  Monetary,
  Angle,
  RelativeHumidity,
  Speed,
  Charge
}

public static class EgaugeRegisterTypeExtensions
{
  public static EgaugeRegisterUnit Unit(this EgaugeRegisterType type)
  {
    return type switch
    {
      EgaugeRegisterType.Irradiance => EgaugeRegisterUnit.WattsPerSquareMeter,
      EgaugeRegisterType.Frequency => EgaugeRegisterUnit.Hertz,
      EgaugeRegisterType.Current => EgaugeRegisterUnit.Ampere,
      EgaugeRegisterType.ReactivePower => EgaugeRegisterUnit.VoltAmpereReactive,
      EgaugeRegisterType.Pressure => EgaugeRegisterUnit.Pascal,
      EgaugeRegisterType.Power => EgaugeRegisterUnit.Watt,
      EgaugeRegisterType.VolumetricFlow => EgaugeRegisterUnit.Mm3ps,
      EgaugeRegisterType.MassFlow => EgaugeRegisterUnit.GramPerSecond,
      EgaugeRegisterType.Resistance => EgaugeRegisterUnit.Ohm,
      EgaugeRegisterType.ApparentPower => EgaugeRegisterUnit.VoltAmpere,
      EgaugeRegisterType.TotalHarmonicDistortion => EgaugeRegisterUnit.Percent,
      EgaugeRegisterType.Temperature => EgaugeRegisterUnit.Celsius,
      EgaugeRegisterType.Voltage => EgaugeRegisterUnit.Volt,
      EgaugeRegisterType.Numeric => EgaugeRegisterUnit.None,
      EgaugeRegisterType.Monetary => EgaugeRegisterUnit.CurrencyPerSecond,
      EgaugeRegisterType.Angle => EgaugeRegisterUnit.Degrees,
      EgaugeRegisterType.RelativeHumidity => EgaugeRegisterUnit.Percent,
      EgaugeRegisterType.Speed => EgaugeRegisterUnit.MeterPerSecond,
      EgaugeRegisterType.Charge => EgaugeRegisterUnit.AmpereHours,
      var _ => default
    };
  }
}

public static class EgaugeRegisterTypeString
{
  public const string Irradiance = "Ee";
  public const string Frequency = "F";
  public const string Current = "I";
  public const string ReactivePower = "PQ";
  public const string Pressure = "Pa";
  public const string Power = "P";
  public const string VolumetricFlow = "Qv";
  public const string MassFlow = "Q";
  public const string Resistance = "R";
  public const string ApparentPower = "S";
  public const string TotalHarmonicDistortion = "THD";
  public const string Temperature = "T";
  public const string Voltage = "V";
  public const string Numeric = "#";
  public const string Monetary = "$";
  public const string Angle = "a";
  public const string RelativeHumidity = "h";
  public const string Speed = "v";
  public const string Charge = "Qe";

  public static EgaugeRegisterType? ToEgaugeRegisterType(
    this string egaugeRegisterTypeString
  )
  {
    return egaugeRegisterTypeString switch
    {
      Irradiance => EgaugeRegisterType.Irradiance,
      Frequency => EgaugeRegisterType.Frequency,
      Current => EgaugeRegisterType.Current,
      ReactivePower => EgaugeRegisterType.ReactivePower,
      Pressure => EgaugeRegisterType.Pressure,
      Power => EgaugeRegisterType.Power,
      VolumetricFlow => EgaugeRegisterType.VolumetricFlow,
      MassFlow => EgaugeRegisterType.MassFlow,
      Resistance => EgaugeRegisterType.Resistance,
      ApparentPower => EgaugeRegisterType.ApparentPower,
      TotalHarmonicDistortion => EgaugeRegisterType.TotalHarmonicDistortion,
      Temperature => EgaugeRegisterType.Temperature,
      Voltage => EgaugeRegisterType.Voltage,
      Numeric => EgaugeRegisterType.Numeric,
      Monetary => EgaugeRegisterType.Monetary,
      Angle => EgaugeRegisterType.Angle,
      RelativeHumidity => EgaugeRegisterType.RelativeHumidity,
      Speed => EgaugeRegisterType.Speed,
      Charge => EgaugeRegisterType.Charge,
      var _ => default
    };
  }
}
