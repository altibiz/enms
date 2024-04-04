using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class SystemAuditEventModelEntityConverter : ModelEntityConverter<
  SystemAuditEventModel, SystemAuditEventEntity>
{
  protected override SystemAuditEventEntity ToEntity(
    SystemAuditEventModel model)
  {
    return model.ToEntity();
  }

  protected override SystemAuditEventModel ToModel(
    SystemAuditEventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class SystemAuditEventModelEntityExtensions
{
  public static SystemAuditEventModel ToModel(
    this SystemAuditEventEntity entity)
  {
    return new SystemAuditEventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Description = entity.Description,
      Audit = entity.Audit.ToModel()
    };
  }

  public static SystemAuditEventEntity ToEntity(
    this SystemAuditEventModel model)
  {
    return new SystemAuditEventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Description = model.Description,
      Audit = model.Audit.ToEntity()
    };
  }
}
