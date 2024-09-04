using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Enms.Client.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsClient(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints
  )
  {
    endpoints.MapControllerRoute(
      "Enms.Client.Controllers.Index.Index",
      "Enms.Client",
      "/",
      new { controller = "Index", action = "Index" }
    );

    endpoints.MapAreaControllerRoute(
      "Enms.Client.Controllers.App.Catchall",
      "Enms.Client",
      "/app/{culture}/{**catchall}",
      new { controller = "App", action = "Catchall" }
    );

    endpoints.MapBlazorHub("/app/{culture}/_blazor");

    return app;
  }
}
