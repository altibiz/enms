using Enms.Business.Conversion.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class RepresentativeEventModelEntityConverter : ModelEntityConverter<
  RepresentativeEventModel, RepresentativeEventEntity>
{
  protected override RepresentativeEventEntity ToEntity(
    RepresentativeEventModel model)
  {
    return model.ToEntity();
  }

  protected override RepresentativeEventModel ToModel(
    RepresentativeEventEntity entity)
  {
    return entity.ToModel();
  }
}

public static class RepresentativeEventModelEntityConverterExtensions
{
  public static RepresentativeEventModel ToModel(
    this RepresentativeEventEntity entity)
  {
    return new RepresentativeEventModel
    {
      Id = entity.Id,
      Title = entity.Title,
      Timestamp = entity.Timestamp,
      Level = entity.Level.ToModel(),
      Content = entity.Content,
      RepresentativeId = entity.RepresentativeId,
      Categories = entity.Categories.Select(c => c.ToModel()).ToHashSet()
    };
  }

  public static RepresentativeEventEntity ToEntity(
    this RepresentativeEventModel model)
  {
    return new RepresentativeEventEntity
    {
      Id = model.Id,
      Title = model.Title,
      Timestamp = model.Timestamp,
      Level = model.Level.ToEntity(),
      Content = model.Content,
      RepresentativeId = model.RepresentativeId,
      Categories = model.Categories.Select(c => c.ToEntity()).ToList()
    };
  }
}
