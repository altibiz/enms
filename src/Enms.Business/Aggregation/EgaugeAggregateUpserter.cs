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
        MeterId = lhs.MeterId,
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
      MeterId = lhs.MeterId,
      Timestamp = lhs.Timestamp,
      Interval = lhs.Interval,
      Count = lhs.Count + rhs.Count
    };
  }
}
