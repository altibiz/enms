using System.ComponentModel.DataAnnotations;
using Enms.Business.Math;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class EgaugeAggregateModel : AggregateModel
{
  [Required]
  public required decimal VoltageL1AnyT0Avg_V { get; set; }

  [Required]
  public required decimal VoltageL2AnyT0Avg_V { get; set; }

  [Required]
  public required decimal VoltageL3AnyT0Avg_V { get; set; }

  [Required]
  public required decimal CurrentL1AnyT0Avg_A { get; set; }

  [Required]
  public required decimal CurrentL2AnyT0Avg_A { get; set; }

  [Required]
  public required decimal CurrentL3AnyT0Avg_A { get; set; }

  [Required]
  public required decimal ActivePowerL1NetT0Avg_W { get; set; }

  [Required]
  public required decimal ActivePowerL2NetT0Avg_W { get; set; }

  [Required]
  public required decimal ActivePowerL3NetT0Avg_W { get; set; }

  [Required]
  public required decimal ApparentPowerL1NetT0Avg_W { get; set; }

  [Required]
  public required decimal ApparentPowerL2NetT0Avg_W { get; set; }

  [Required]
  public required decimal ApparentPowerL3NetT0Avg_W { get; set; }

  public override TariffMeasure<decimal> Current_A
  {
    get
    {
      return new UnaryTariffMeasure<decimal>(
        new AnyDuplexMeasure<decimal>(
          new TriPhasicMeasure<decimal>(
            CurrentL1AnyT0Avg_A,
            CurrentL2AnyT0Avg_A,
            CurrentL3AnyT0Avg_A
          )
        )
      );
    }
  }

  public override TariffMeasure<decimal> Voltage_V
  {
    get
    {
      return new UnaryTariffMeasure<decimal>(
        new AnyDuplexMeasure<decimal>(
          new TriPhasicMeasure<decimal>(
            VoltageL1AnyT0Avg_V,
            VoltageL2AnyT0Avg_V,
            VoltageL3AnyT0Avg_V
          )
        )
      );
    }
  }

  public override TariffMeasure<decimal> ActivePower_W
  {
    get
    {
      return new CompositeTariffMeasure<decimal>(
      [
        base.ActivePower_W,
        new UnaryTariffMeasure<decimal>(
          new NetDuplexMeasure<decimal>(
            new TriPhasicMeasure<decimal>(
              ActivePowerL1NetT0Avg_W,
              ActivePowerL2NetT0Avg_W,
              ActivePowerL3NetT0Avg_W
            )
          )
        )
      ]);
    }
  }

  public override TariffMeasure<decimal> ApparentPower_VA
  {
    get
    {
      return new CompositeTariffMeasure<decimal>(
      [
        base.ApparentPower_VA,
        new UnaryTariffMeasure<decimal>(
          new NetDuplexMeasure<decimal>(
            new TriPhasicMeasure<decimal>(
              ApparentPowerL1NetT0Avg_W,
              ApparentPowerL2NetT0Avg_W,
              ApparentPowerL3NetT0Avg_W
            )
          )
        )
      ]);
    }
  }

  public override SpanningMeasure<decimal> ActiveEnergySpan_Wh
  {
    get { return SpanningMeasure<decimal>.Null; }
  }

  public override SpanningMeasure<decimal> ReactiveEnergySpan_VARh
  {
    get { return SpanningMeasure<decimal>.Null; }
  }

  public override SpanningMeasure<decimal> ApparentEnergySpan_VAh
  {
    get { return SpanningMeasure<decimal>.Null; }
  }
}
