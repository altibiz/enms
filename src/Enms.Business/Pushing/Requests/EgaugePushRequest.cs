namespace Enms.Business.Pushing.Requests;

public readonly record struct EgaugePushRequest(
  List<EgaugeRegister> Registers,
  List<EgaugeRange> Ranges
);

public readonly record struct EgaugeRegister(
  string Name,
  string Type,
  decimal Did
);

public readonly record struct EgaugeRange(
  string Ts,
  decimal Delta,
  List<List<string>> Rows
);
