using Enms.Business.Capabilities;
using Enms.Business.Capabilities.Abstractions;
using Enms.Business.Models.Base;

namespace Enms.Business.Models;

public class EgaugeLineModel : LineModel
{
  public override ILineCapabilities Capabilities
  {
    get { return new EgaugeLineCapabilities(); }
  }
}
