using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Enums;

public enum OperationModel
{
  Sum,
  Average
}

public static class OperationExtensions
{
  public static decimal ChartValue(
    this IMeasurement measurement,
    MeasureModel measure,
    OperationModel? operation)
  {
    var byTariff = measure switch
    {
      MeasureModel.Current => measurement.Current_A,
      MeasureModel.Voltage => measurement.Voltage_V,
      MeasureModel.ActivePower => measurement.ActivePower_W,
      MeasureModel.ReactivePower => measurement.ReactivePower_VAR,
      MeasureModel.ApparentPower => measurement.ApparentPower_VA,
      MeasureModel.ActiveEnergy => measurement.ActiveEnergy_Wh,
      MeasureModel.ReactiveEnergy => measurement.ReactiveEnergy_VARh,
      MeasureModel.ApparentEnergy => measurement.ApparentEnergy_VAh,
      _ => throw new ArgumentOutOfRangeException(nameof(measure), measure, null)
    };

    var byDuplex = byTariff.TariffUnary();

    var byPhase = byDuplex.DuplexAny();

    var result = operation switch
    {
      OperationModel.Sum => byPhase.PhaseSum(),
      OperationModel.Average => byPhase.PhaseAverage(),
      _ => byPhase.PhaseSum()
    };

    return result;
  }

  public static string ToTitle(this OperationModel operation)
  {
    return operation switch
    {
      OperationModel.Sum => "Sum",
      OperationModel.Average => "Average",
      _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
    };
  }
}
