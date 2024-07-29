using System.Linq.Expressions;
using Enms.Business.Aggregation.Base;
using Enms.Business.Models;
using Enms.Data.Entities;

namespace Enms.Business.Aggregation;

public class EgaugeAggregateUpserter : AggregateUpserter<EgaugeAggregateModel,
  EgaugeAggregateEntity>
{
  protected override
    Expression<Func<EgaugeAggregateEntity, EgaugeAggregateEntity,
      EgaugeAggregateEntity>> UpsertConcreteEntity
  {
    get
    {
      return (lhs, rhs) => new EgaugeAggregateEntity
      {
        LineId = lhs.LineId,
        Timestamp = lhs.Timestamp,
        Interval = lhs.Interval,
        Count = lhs.Count + rhs.Count
      };
    }
  }

  protected override EgaugeAggregateModel UpsertConcreteModel(
    EgaugeAggregateModel lhs,
    EgaugeAggregateModel rhs)
  {
    return new EgaugeAggregateModel
    {
      LineId = lhs.LineId,
      Timestamp = lhs.Timestamp,
      Interval = lhs.Interval,
      Count = lhs.Count + rhs.Count,
      VoltageL1AnyT0Avg_V =
        (lhs.VoltageL1AnyT0Avg_V * lhs.Count +
        rhs.VoltageL1AnyT0Avg_V * rhs.Count)
        / (rhs.Count + lhs.Count),
      VoltageL2AnyT0Avg_V =
        (lhs.VoltageL2AnyT0Avg_V * lhs.Count +
        rhs.VoltageL2AnyT0Avg_V * rhs.Count)
        / (rhs.Count + lhs.Count),
      VoltageL3AnyT0Avg_V =
        (lhs.VoltageL3AnyT0Avg_V * lhs.Count +
        rhs.VoltageL3AnyT0Avg_V * rhs.Count)
        / (rhs.Count + lhs.Count),
      CurrentL1AnyT0Avg_A =
        (lhs.CurrentL1AnyT0Avg_A * lhs.Count +
        rhs.CurrentL1AnyT0Avg_A * rhs.Count)
        / (rhs.Count + lhs.Count),
      CurrentL2AnyT0Avg_A =
        (lhs.CurrentL2AnyT0Avg_A * lhs.Count +
        rhs.CurrentL2AnyT0Avg_A * rhs.Count)
        / (rhs.Count + lhs.Count),
      CurrentL3AnyT0Avg_A =
        (lhs.CurrentL3AnyT0Avg_A * lhs.Count +
        rhs.CurrentL3AnyT0Avg_A * rhs.Count)
        / (rhs.Count + lhs.Count),
      ActivePowerL1NetT0Avg_W =
        (lhs.ActivePowerL1NetT0Avg_W * lhs.Count +
        rhs.ActivePowerL1NetT0Avg_W * rhs.Count)
        / (rhs.Count + lhs.Count),
      ActivePowerL2NetT0Avg_W =
        (lhs.ActivePowerL2NetT0Avg_W * lhs.Count +
        rhs.ActivePowerL2NetT0Avg_W * rhs.Count)
        / (rhs.Count + lhs.Count),
      ActivePowerL3NetT0Avg_W =
        (lhs.ActivePowerL3NetT0Avg_W * lhs.Count +
        rhs.ActivePowerL3NetT0Avg_W * rhs.Count)
        / (rhs.Count + lhs.Count),
      ApparentPowerL1NetT0Avg_W =
        (lhs.ApparentPowerL1NetT0Avg_W * lhs.Count +
        rhs.ApparentPowerL1NetT0Avg_W * rhs.Count)
        / (rhs.Count + lhs.Count),
      ApparentPowerL2NetT0Avg_W =
        (lhs.ApparentPowerL2NetT0Avg_W * lhs.Count +
        rhs.ApparentPowerL2NetT0Avg_W * rhs.Count)
        / (rhs.Count + lhs.Count),
      ApparentPowerL3NetT0Avg_W =
        (lhs.ApparentPowerL3NetT0Avg_W * lhs.Count +
        rhs.ApparentPowerL3NetT0Avg_W * rhs.Count)
        / (rhs.Count + lhs.Count),
    };
  }
}
