using System.IO.Compression;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Pushing.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enms.Business.Controllers;

[IgnoreAntiforgeryToken]
public class IotController(
  AgnosticPushRequestMeasurementConverter pushRequestMeasurementConverter,
  IMeasurementPusher measurementPusher
) : Controller
{
  [HttpPost]
  public async Task<IActionResult> Push(string id)
  {
    var message = new GZipStream(Request.Body, CompressionMode.Decompress);

    var measurements = await pushRequestMeasurementConverter.ToMeasurements(
      id,
      DateTimeOffset.UtcNow,
      message
    );

    await measurementPusher.Push(measurements);

    return Ok();
  }
}
