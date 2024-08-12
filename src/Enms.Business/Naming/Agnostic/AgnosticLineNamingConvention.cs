using Enms.Business.Naming.Abstractions;

namespace Enms.Business.Naming.Agnostic;

public class AgnosticLineNamingConvention(IServiceProvider serviceProvider)
{
  public string LineIdPrefixForLineType(Type lineType)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => x.LineType == lineType)
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {lineType}");

    return lineNamingConvention.LineIdPrefix;
  }

  public string LineIdPrefixForMeasurementType(Type measurementType)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => x.MeasurementType == measurementType)
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {measurementType}");

    return lineNamingConvention.LineIdPrefix;
  }

  public string LineIdPrefixForAggregateType(Type aggregateType)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => x.AggregateType == aggregateType)
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {aggregateType}");

    return lineNamingConvention.LineIdPrefix;
  }

  public string LineIdPrefixForMeasurementValidatorType(Type measurementValidatorType)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => x.MeasurementValidatorType == measurementValidatorType)
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {measurementValidatorType}");

    return lineNamingConvention.LineIdPrefix;
  }

  public string LineIdPrefixForMeterType(Type meterType)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => x.MeterType == meterType)
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {meterType}");

    return lineNamingConvention.LineIdPrefix;
  }

  public Type LineTypeForLineId(string lineId)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => lineId.StartsWith(x.LineIdPrefix))
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {lineId}");

    return lineNamingConvention.LineType;
  }

  public Type MeasurementTypeForLineId(string lineId)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => lineId.StartsWith(x.LineIdPrefix))
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {lineId}");

    return lineNamingConvention.MeasurementType;
  }

  public Type AggregateTypeForLineId(string lineId)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => lineId.StartsWith(x.LineIdPrefix))
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {lineId}");

    return lineNamingConvention.AggregateType;
  }

  public Type MeasurementValidatorTypeForLineId(string lineId)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => lineId.StartsWith(x.LineIdPrefix))
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {lineId}");

    return lineNamingConvention.MeasurementValidatorType;
  }

  public Type MeterTypeForLineId(string lineId)
  {
    var lineNamingConvention = serviceProvider
      .GetServices<ILineNamingConvention>()
      .FirstOrDefault(x => lineId.StartsWith(x.LineIdPrefix))
      ?? throw new InvalidOperationException(
        $"No LineNamingConvention found for {lineId}");

    return lineNamingConvention.MeterType;
  }
}
