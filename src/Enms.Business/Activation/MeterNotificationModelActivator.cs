using Enms.Business.Activation.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;

namespace Enms.Business.Activation;

public class
  MeterNotificationModelActivator : ModelActivator<MeterNotificationModel>
{
  public override MeterNotificationModel ActivateConcrete()
  {
    return New();
  }

  public static MeterNotificationModel New()
  {
    return new MeterNotificationModel
    {
      Id = default!,
      Title = "",
      Summary = "",
      Content = "",
      Timestamp = DateTimeOffset.UtcNow,
      MeterId = default!,
      ResolvedOn = null,
      Topics = new HashSet<TopicModel>()
    };
  }
}
