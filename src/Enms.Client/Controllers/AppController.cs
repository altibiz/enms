using System.Globalization;
using Enms.Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Enms.Client.Controllers;

public class AppController : Controller
{
  public IActionResult Catchall([FromRoute] string culture)
  {
    try
    {
      return View(new AppViewModel { Culture = new CultureInfo(culture) });
    }
    catch (CultureNotFoundException ex)
    {
      return BadRequest(ex);
    }
  }
}
