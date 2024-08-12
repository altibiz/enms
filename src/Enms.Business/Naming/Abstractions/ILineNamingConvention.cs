namespace Enms.Business.Naming.Abstractions;

public interface ILineNamingConvention
{
  public string LineIdPrefix { get; }

  public Type LineType { get; }

  public Type MeasurementType { get; }

  public Type AggregateType { get; }

  public Type MeasurementValidatorType { get; }

  public Type MeterType { get; }
}
