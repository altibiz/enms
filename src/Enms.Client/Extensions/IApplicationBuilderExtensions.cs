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
    endpoints.MapBlazorHub("/app/{culture}/_blazor");

    endpoints.MapEnmsClientRoute(
      "/",
      typeof(IndexController),
      nameof(IndexController.Index)
    );

    endpoints.MapEnmsClientRoute(
      "/app/{culture}/{**catchall}",
      typeof(AppController),
      nameof(AppController.Catchall)
    );

    return app;
  }

  private static void MapEnmsClientRoute(
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
        controller = controller.Name.Remove(
          controller.Name.Length -
          nameof(Microsoft.AspNetCore.Mvc.Controller).Length,
          nameof(Microsoft.AspNetCore.Mvc.Controller).Length),
        action
      }
    );
  }
}
