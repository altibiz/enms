using System.ComponentModel.DataAnnotations;
using Enms.Business.Math;

namespace Enms.Business.Models.Abstractions;

public interface IMeasurement : IValidatableObject, IReadonly
{
  public string MeterId { get; }

  public string LineId { get; }

  public DateTimeOffset Timestamp { get; }

  public TariffMeasure<decimal> Current_A { get; }

  public TariffMeasure<decimal> Voltage_V { get; }

  public TariffMeasure<decimal> ActivePower_W { get; }

  public TariffMeasure<decimal> ReactivePower_VAR { get; }

  public TariffMeasure<decimal> ApparentPower_VA { get; }

  public TariffMeasure<decimal> ActiveEnergy_Wh { get; }

  public TariffMeasure<decimal> ReactiveEnergy_VARh { get; }

  public TariffMeasure<decimal> ApparentEnergy_VAh { get; }
}
