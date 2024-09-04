using Microsoft.AspNetCore.Mvc;

namespace Enms.Client.Controllers;

public class IndexController : Controller
{
  public IActionResult Index()
  {
    return RedirectToAction("Index", "App", new { culture = "en" });
  }
}