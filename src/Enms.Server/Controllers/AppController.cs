using System.Globalization;
using Enms.Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Enms.Server.Controllers;

public class AppController : Controller
{
  public IActionResult Catchall([FromRoute] string culture)
  {
    CultureInfo? cultureInfo = null;
    try
    {
      cultureInfo = new CultureInfo(culture);
    }
    catch (CultureNotFoundException ex)
    {
      return BadRequest(ex);
    }

    return View(new AppViewModel { Culture = cultureInfo });
  }
}
