using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
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
    var message = await JsonSerializer.DeserializeAsync<JsonNode>(
      new GZipStream(Request.Body, CompressionMode.Decompress));

    if (message == null)
    {
      return BadRequest();
    }

    if (!await _iotHandler.Authorize(id, message))
    {
      return Unauthorized();
    }

    await _iotHandler.OnPush(id, message);

    return Ok();
  }
}
