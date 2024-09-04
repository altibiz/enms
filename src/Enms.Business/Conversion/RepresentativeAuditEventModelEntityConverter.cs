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
      Content = entity.Content,
      Audit = entity.Audit.ToModel(),
      RepresentativeId = entity.RepresentativeId,
      Categories = entity.Categories.Select(c => c.ToModel()).ToHashSet()
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
      Content = model.Content,
      Audit = model.Audit.ToEntity(),
      RepresentativeId = model.RepresentativeId,
      Categories = model.Categories.Select(c => c.ToEntity()).ToList()
    };
  }
}
