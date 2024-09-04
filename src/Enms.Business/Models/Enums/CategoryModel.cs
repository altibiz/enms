using Enms.Data.Entities.Enums;

namespace Enms.Business.Models.Enums;

public enum CategoryModel
{
  All,
  Messenger,
  MessengerPush
}

public static class CategoryModelExtensions
{
  public static CategoryModel ToModel(this CategoryEntity entity)
  {
    return entity switch
    {
      CategoryEntity.All => CategoryModel.All,
      CategoryEntity.Meter => CategoryModel.Messenger,
      CategoryEntity.MeterPush => CategoryModel.MessengerPush,
      _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
    };
  }

  public static CategoryEntity ToEntity(this CategoryModel model)
  {
    return model switch
    {
      CategoryModel.All => CategoryEntity.All,
      CategoryModel.Messenger => CategoryEntity.Meter,
      CategoryModel.MessengerPush => CategoryEntity.MeterPush,
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }

  public static string ToTitle(this CategoryModel model)
  {
    return model switch
    {
      CategoryModel.All => "General",
      CategoryModel.Messenger => "Messenger",
      CategoryModel.MessengerPush => "Messenger push",
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };
  }
}
