namespace Enms.Business.Iot;

public readonly record struct EgaugeRegister(
  EgaugeRegisterType Type,
  EgaugeRegisterUnit Unit,
  decimal Value
);
