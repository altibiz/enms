using System.Linq.Expressions;
using Enms.Business.Models.Abstractions;

namespace Enms.Business.Aggregation.Abstractions;

public interface IAggregateUpserter
{
  LambdaExpression UpsertEntity { get; }

  bool CanUpsertModel(Type modelType);

  bool CanUpsertEntity(Type entityType);

  IAggregate UpsertModel(IAggregate lhs, IAggregate rhs);
}
