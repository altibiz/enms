using System.IO.Compression;
using Enms.Business.Iot;
using Microsoft.AspNetCore.Mvc;

namespace Enms.Server.Controllers;

[IgnoreAntiforgeryToken]
public class IotController : Controller
{
  private readonly EnmsIotHandler _iotHandler;

  public IotController(EnmsIotHandler iotHandler)
  {
    _iotHandler = iotHandler;
  }

  [HttpPost]
  public async Task<IActionResult> Push(string id)
  {
    var message = new GZipStream(Request.Body, CompressionMode.Decompress);

    await _iotHandler.OnPush(id, message);

    return Ok();
  }
}
