using System.IO.Compression;
using Enms.Business.Conversion.Agnostic;
using Enms.Business.Observers.Abstractions;
using Enms.Business.Observers.EventArgs;
using Microsoft.AspNetCore.Mvc;

namespace Enms.Business.Controllers;

[IgnoreAntiforgeryToken]
public class IotController(
  AgnosticPushRequestMeasurementConverter pushRequestMeasurementConverter,
  IMeasurementPublisher publisher
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

    publisher.PublishPush(new MeasurementPushEventArgs(measurements.ToList()));

    return Ok();
  }
}
