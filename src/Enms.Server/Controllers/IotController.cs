using Enms.Business.Iot;
using Microsoft.AspNetCore.Mvc;

namespace Enms.Server.Controllers;

public class IotController : ControllerBase
{
  private readonly EnmsIotHandler _iotHandler;

  public IotController(EnmsIotHandler iotHandler)
  {
    _iotHandler = iotHandler;
  }

  public async Task<IActionResult> Push(
    [FromRoute] string id,
    [FromBody] string message)
  {
    await _iotHandler.OnPush(id, message);

    return Ok();
  }
}
