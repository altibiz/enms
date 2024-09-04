using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class MeterEventModel : EventModel
{
  public required string MeterId { get; set; }
}
