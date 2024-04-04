using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Abstractions;

public interface IAuditEvent : IEvent
{
  public AuditModel Audit { get; }
}
