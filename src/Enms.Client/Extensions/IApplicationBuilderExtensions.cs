using Enms.Client.Controllers;
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
    endpoints.MapAreaControllerRoute(
      "/",
      typeof(IndexController),
      nameof(IndexController.Index)
    );

    endpoints.MapAreaControllerRoute(
      "/app/{culture}/{**catchall}",
      typeof(AppController),
      nameof(AppController.Catchall)
    );

    endpoints.MapBlazorHub("/app/{culture}/_blazor");

    return app;
  }

  private static void MapAreaControllerRoute(
    this IEndpointRouteBuilder endpoints,
    string pattern,
    Type controller,
    string action)
  {
    endpoints.MapControllerRoute(
      $"{controller.Namespace}.{controller.Name}.{action}",
      pattern,
      defaults: new
      {
        controller = controller.Name.Remove(
          nameof(Microsoft.AspNetCore.Mvc.Controller).Length),
        action
      }
    );
  }
}
