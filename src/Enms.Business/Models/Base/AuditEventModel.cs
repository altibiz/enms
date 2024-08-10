using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;

namespace Enms.Business.Models.Base;

public class AuditEventModel : EventModel, IAuditEvent
{
  public required AuditModel Audit { get; init; }
}
