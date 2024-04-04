using Microsoft.AspNetCore.Mvc;

namespace Enms.Server.Controllers;

public class AppController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}
