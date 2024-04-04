using System.Linq.Expressions;
using Enms.Business.Models.Abstractions;
using Enms.Data.Entities.Abstractions;

namespace Enms.Business.Conversion.Abstractions;

public interface IAggregateUpserter
{
  Expression<Func<IAggregateEntity, IAggregateEntity, IAggregateEntity>>
    UpsertEntity { get; }

  bool CanUpsertModel(Type modelType);

  bool CanUpsertEntity(Type entityType);

  IAggregate UpsertModel(IAggregate lhs, IAggregate rhs);
}
