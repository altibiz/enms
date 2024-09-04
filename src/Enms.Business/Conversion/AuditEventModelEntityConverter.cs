using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Base;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;

namespace Enms.Business.Conversion;

public class AuditEventModelEntityConverter : ModelEntityConverter<
  AuditEventModel, AuditEventEntity>
{
  protected override AuditEventEntity ToEntity(AuditEventModel model)
  {
    return model.ToEntity();
  }

  protected override AuditEventModel ToModel(AuditEventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class AuditEventModelEntityConverterExtensions
{
  public static AuditEventModel ToModel(this AuditEventEntity entity)
  {
    if (entity is RepresentativeAuditEventEntity representativeAuditEventEntity)
    {
      return representativeAuditEventEntity.ToModel();
    }

    if (entity is SystemAuditEventEntity systemAuditEventEntity)
    {
      return systemAuditEventEntity.ToModel();
    }

    return new AuditEventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Content = entity.Content,
      Audit = entity.Audit.ToModel(),
      Categories = entity.Categories.Select(c => c.ToModel()).ToHashSet()
    };
  }

  public static AuditEventEntity ToEntity(this AuditEventModel model)
  {
    if (model is RepresentativeAuditEventModel representativeAuditEventModel)
    {
      return representativeAuditEventModel.ToEntity();
    }

    if (model is SystemAuditEventModel systemAuditEventModel)
    {
      return systemAuditEventModel.ToEntity();
    }

    return new AuditEventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Content = model.Content,
      Audit = model.Audit.ToEntity(),
      Categories = model.Categories.Select(c => c.ToEntity()).ToList()
    };
  }
}
