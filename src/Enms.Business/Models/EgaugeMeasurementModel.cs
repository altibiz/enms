using System.ComponentModel.DataAnnotations;
using Enms.Business.Math;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class
  EgaugeMeasurementModel : MeasurementModel<EgaugeMeasurementValidatorModel>
{
  [Required]
  public required decimal VoltageL1AnyT0_V { get; set; }

  [Required]
  public required decimal VoltageL2AnyT0_V { get; set; }

  [Required]
  public required decimal VoltageL3AnyT0_V { get; set; }

  [Required]
  public required decimal CurrentL1AnyT0_A { get; set; }

  [Required]
  public required decimal CurrentL2AnyT0_A { get; set; }

  [Required]
  public required decimal CurrentL3AnyT0_A { get; set; }

  [Required]
  public required decimal ActivePowerL1NetT0_W { get; set; }

  [Required]
  public required decimal ActivePowerL2NetT0_W { get; set; }

  [Required]
  public required decimal ActivePowerL3NetT0_W { get; set; }

  [Required]
  public required decimal ApparentPowerL1NetT0_W { get; set; }

  [Required]
  public required decimal ApparentPowerL2NetT0_W { get; set; }

  [Required]
  public required decimal ApparentPowerL3NetT0_W { get; set; }

  public override TariffMeasure<decimal> Current_A
  {
    get
    {
      return new UnaryTariffMeasure<decimal>(
        new AnyDuplexMeasure<decimal>(
          new TriPhasicMeasure<decimal>(
            CurrentL1AnyT0_A,
            CurrentL2AnyT0_A,
            CurrentL3AnyT0_A
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
            VoltageL1AnyT0_V,
            VoltageL2AnyT0_V,
            VoltageL3AnyT0_V
          )
        )
      );
    }
  }

  public override TariffMeasure<decimal> ActivePower_W
  {
    get
    {
      return new UnaryTariffMeasure<decimal>(
        new NetDuplexMeasure<decimal>(
          new TriPhasicMeasure<decimal>(
            ActivePowerL1NetT0_W,
            ActivePowerL2NetT0_W,
            ActivePowerL3NetT0_W
          )
        )
      );
    }
  }

  public override TariffMeasure<decimal> ReactivePower_VAR
  {
    get { return TariffMeasure<decimal>.Null; }
  }

  public override TariffMeasure<decimal> ApparentPower_VA
  {
    get
    {
      return new UnaryTariffMeasure<decimal>(
        new NetDuplexMeasure<decimal>(
          new TriPhasicMeasure<decimal>(
            ApparentPowerL1NetT0_W,
            ApparentPowerL2NetT0_W,
            ApparentPowerL3NetT0_W
          )
        )
      );
    }
  }

  public override TariffMeasure<decimal> ActiveEnergy_Wh
  {
    get { return TariffMeasure<decimal>.Null; }
  }

  public override TariffMeasure<decimal> ReactiveEnergy_VARh
  {
    get { return TariffMeasure<decimal>.Null; }
  }

  public override TariffMeasure<decimal> ApparentEnergy_VAh
  {
    get { return TariffMeasure<decimal>.Null; }
  }
}
