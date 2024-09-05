using Microsoft.AspNetCore.Mvc;

namespace Enms.Client.Controllers;

public class IndexController : Controller
{
  public IActionResult Index()
  {
    return RedirectToAction(
      nameof(AppController).Remove(
        nameof(AppController).Length - nameof(Controller).Length,
        nameof(Controller).Length
      ),
      nameof(AppController.Catchall),
      new { culture = "en" }
    );
  }
}
