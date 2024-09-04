using Enms.Business.Models;
using Enms.Business.Naming.Base;

namespace Enms.Business.Naming;

public class EgaugeLineNamingConvention : LineNamingConvention
{
  public override string LineIdPrefix
  {
    get { return "e"; }
  }

  public override string MeterIdPrefix
  {
    get { return "egauge"; }
  }

  public override Type LineType
  {
    get { return typeof(EgaugeLineModel); }
  }

  public override Type MeasurementType
  {
    get { return typeof(EgaugeMeasurementModel); }
  }

  public override Type AggregateType
  {
    get { return typeof(EgaugeAggregateModel); }
  }

  public override Type MeasurementValidatorType
  {
    get { return typeof(EgaugeMeasurementValidatorModel); }
  }

  public override Type MeterType
  {
    get { return typeof(EgaugeMeterModel); }
  }
}
