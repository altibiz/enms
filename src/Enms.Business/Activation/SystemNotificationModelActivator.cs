using Enms.Business.Activation.Base;
using Enms.Business.Models;
using Enms.Business.Models.Enums;

namespace Enms.Business.Activation;

public class SystemNotificationModelActivator
  : ModelActivator<SystemNotificationModel>
{
  public override SystemNotificationModel ActivateConcrete()
  {
    return new()
    {
      Id = default!,
      Title = "",
      Summary = "",
      Content = "",
      Timestamp = DateTimeOffset.UtcNow,
      Topics = new HashSet<TopicModel>(),
    };
  }
}
