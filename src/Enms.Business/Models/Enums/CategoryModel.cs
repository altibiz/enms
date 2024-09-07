using Enms.Data.Entities.Enums;

namespace Enms.Business.Models.Enums;

public enum CategoryModel
{
  All,
  Audit,
  Meter,
  MeterPush
}

public static class CategoryModelExtensions
{
  public static CategoryModel ToModel(this CategoryEntity entity)
  {
    return entity switch
    {
      CategoryEntity.All => CategoryModel.All,
      CategoryEntity.Audit => CategoryModel.Audit,
      CategoryEntity.Meter => CategoryModel.Meter,
      CategoryEntity.MeterPush => CategoryModel.MeterPush,
      _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
    };
  }

  public static CategoryEntity ToEntity(this CategoryModel model)
  {
    return model switch
    {
      CategoryModel.All => CategoryEntity.All,
      CategoryModel.Audit => CategoryEntity.Audit,
      CategoryModel.Meter => CategoryEntity.Meter,
      CategoryModel.MeterPush => CategoryEntity.MeterPush,
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }

  public static string ToTitle(this CategoryModel model)
  {
    return model switch
    {
      CategoryModel.All => "General",
      CategoryModel.Meter => "Messenger",
      CategoryModel.MeterPush => "Messenger push",
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }
}
