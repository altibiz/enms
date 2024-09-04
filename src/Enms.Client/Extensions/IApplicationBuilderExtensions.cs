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
      typeof(Controllers.IndexController),
      nameof(Controllers.IndexController.Index)
    );

    endpoints.MapAreaControllerRoute(
      "/app/{culture}/{**catchall}",
      typeof(Controllers.AppController),
      nameof(Controllers.AppController.Catchall)
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
      name: $"{controller.Namespace}.{controller.Name}.{action}",
      pattern: pattern,
      defaults: new
      {
        controller = controller.Name,
        action = action
      }
    );
  }
}
