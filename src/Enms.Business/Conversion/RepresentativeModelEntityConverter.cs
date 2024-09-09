using Enms.Business.Conversion.Base;
using Enms.Business.Conversion.Complex;
using Enms.Business.Models;
using Enms.Business.Models.Enums;
using Enms.Data.Entities;

namespace Enms.Business.Conversion;

public class RepresentativeModelEntityConverter : ModelEntityConverter<
  RepresentativeModel, RepresentativeEntity>
{
  protected override RepresentativeEntity ToEntity(RepresentativeModel model)
  {
    return model.ToEntity();
  }

  protected override RepresentativeModel ToModel(RepresentativeEntity entity)
  {
    return entity.ToModel();
  }
}

public static class RepresentativeModelEntityConverterExtensions
{
  public static RepresentativeEntity ToEntity(this RepresentativeModel model)
  {
    return new RepresentativeEntity
    {
      Id = model.Id,
      Title = model.Title,
      CreatedOn = model.CreatedOn,
      CreatedById = model.CreatedById,
      LastUpdatedOn = model.LastUpdatedOn,
      LastUpdatedById = model.LastUpdatedById,
      IsDeleted = model.IsDeleted,
      DeletedOn = model.DeletedOn,
      DeletedById = model.DeletedById,
      Role = model.Role.ToEntity(),
      Topics = model.Topics.Select(x => x.ToEntity()).ToList(),
      PhysicalPerson = model.PhysicalPerson.ToEntity(),
      NetworkUserId = model.NetworkUserId
    };
  }

  public static RepresentativeModel ToModel(this RepresentativeEntity entity)
  {
    return new RepresentativeModel
    {
      Id = entity.Id,
      Title = entity.Title,
      CreatedOn = entity.CreatedOn,
      CreatedById = entity.CreatedById,
      LastUpdatedOn = entity.LastUpdatedOn,
      LastUpdatedById = entity.LastUpdatedById,
      IsDeleted = entity.IsDeleted,
      DeletedOn = entity.DeletedOn,
      DeletedById = entity.DeletedById,
      Role = entity.Role.ToModel(),
      Topics = entity.Topics.Select(x => x.ToModel()).ToHashSet(),
      PhysicalPerson = entity.PhysicalPerson.ToModel(),
      NetworkUserId = entity.NetworkUserId
    };
  }
}
