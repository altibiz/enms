using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class RepresentativeAuditEventModelEntityConverter : ModelEntityConverter
  <RepresentativeAuditEventModel, RepresentativeAuditEventEntity>
{
  protected override RepresentativeAuditEventEntity ToEntity(
    RepresentativeAuditEventModel model)
  {
    return model.ToEntity();
  }

  protected override RepresentativeAuditEventModel ToModel(
    RepresentativeAuditEventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class RepresentativeAuditEventModelEntityConverterExtensions
{
  public static RepresentativeAuditEventModel ToModel(
    this RepresentativeAuditEventEntity entity)
  {
    return new RepresentativeAuditEventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Description = entity.Description,
      Audit = entity.Audit.ToModel(),
      RepresentativeId = entity.RepresentativeId
    };
  }

  public static RepresentativeAuditEventEntity ToEntity(
    this RepresentativeAuditEventModel model)
  {
    return new RepresentativeAuditEventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Description = model.Description,
      Audit = model.Audit.ToEntity(),
      RepresentativeId = model.RepresentativeId
    };
  }
}
