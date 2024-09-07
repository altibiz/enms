using Enms.Business.Conversion.Base;
using Enms.Business.Models.Joins;
using Enms.Data.Entities.Joins;

namespace Enms.Business.Conversion.Joins;

public class NotificationRecipientModelEntityConverter : ModelEntityConverter<
  NotificationRecipientModel,
  NotificationRecipientEntity>
{
  protected override NotificationRecipientEntity ToEntity(
    NotificationRecipientModel model)
  {
    return model.ToEntity();
  }

  protected override NotificationRecipientModel ToModel(
    NotificationRecipientEntity entity)
  {
    return entity.ToModel();
  }
}

public static class NotificationRecipientModelEntityConverterExtensions
{
  public static NotificationRecipientEntity ToEntity(
    this NotificationRecipientModel model)
  {
    return new NotificationRecipientEntity
    {
      NotificationId = model.NotificationId,
      RecipientId = model.RepresentativeId,
      SeenOn = model.SeenOn
    };
  }

  public static NotificationRecipientModel ToModel(
    this NotificationRecipientEntity entity)
  {
    return new NotificationRecipientModel
    {
      NotificationId = entity.NotificationId,
      RepresentativeId = entity.RecipientId,
      SeenOn = entity.SeenOn
    };
  }
}
