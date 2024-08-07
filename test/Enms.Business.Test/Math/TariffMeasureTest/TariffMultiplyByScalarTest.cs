using Enms.Business.Math;

namespace Enms.Business.Test.Math.TariffMeasureTest;

public class TariffMultiplyByScalarTest
{
  public static readonly
    TheoryData<TariffMeasure<decimal>, decimal, TariffMeasure<decimal>>
    TariffMeasuresMultiply = new()
    {
      {
        new UnaryTariffMeasure<decimal>(
          new ImportExportDuplexMeasure<decimal>(
            new SinglePhasicMeasureSum<decimal>(5),
            new SinglePhasicMeasureSum<decimal>(3))),
        2,
        new UnaryTariffMeasure<decimal>(
          new ImportExportDuplexMeasure<decimal>(
            new SinglePhasicMeasureSum<decimal>(10),
            new SinglePhasicMeasureSum<decimal>(6)))
      },

      {
        new UnaryTariffMeasure<decimal>(
          new ImportExportDuplexMeasure<decimal>(
            new TriPhasicMeasure<decimal>(1, 2, 3),
            new TriPhasicMeasure<decimal>(4, 5, 6))),
        2,
        new UnaryTariffMeasure<decimal>(
          new ImportExportDuplexMeasure<decimal>(
            new TriPhasicMeasure<decimal>(2, 4, 6),
            new TriPhasicMeasure<decimal>(8, 10, 12)))
      },

      {
        new BinaryTariffMeasure<decimal>(
          new ImportExportDuplexMeasure<decimal>(
            new SinglePhasicMeasureSum<decimal>(1),
            new SinglePhasicMeasureSum<decimal>(2)),
          new ImportExportDuplexMeasure<decimal>(
            new SinglePhasicMeasureSum<decimal>(3),
            new SinglePhasicMeasureSum<decimal>(4))),
        2,
        new BinaryTariffMeasure<decimal>(
          new ImportExportDuplexMeasure<decimal>(
            new SinglePhasicMeasureSum<decimal>(2),
            new SinglePhasicMeasureSum<decimal>(4)),
          new ImportExportDuplexMeasure<decimal>(
            new SinglePhasicMeasureSum<decimal>(6),
            new SinglePhasicMeasureSum<decimal>(8)))
      },

      { new NullTariffMeasure<decimal>(), 2, new NullTariffMeasure<decimal>() }
    };

  [Theory]
  [MemberData(nameof(TariffMeasuresMultiply))]
  public void Multiply_ReturnsExpectedResult(
    TariffMeasure<decimal> measure,
    decimal multiplier,
    TariffMeasure<decimal> expected)
  {
    var result = measure.Multiply(multiplier);

    Assert.Equal(expected, result);
  }
}
