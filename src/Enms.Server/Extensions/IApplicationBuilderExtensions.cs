using Enms.Server.Controllers;

namespace Enms.Server.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsServer(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints
  )
  {
    endpoints.MapEnmsServerRoute(
      "/iot/push/{id}",
      typeof(IotController),
      nameof(IotController.Push)
    );

    endpoints.MapEnmsServerRoute(
      "/",
      typeof(IndexController),
      nameof(IndexController.Index)
    );

    endpoints.MapEnmsServerRoute(
      "/app/{culture}/{**catchall}",
      typeof(AppController),
      nameof(AppController.Catchall)
    );

    endpoints.MapBlazorHub("/app/{culture}/_blazor");

    return app;
  }

  private static void MapEnmsServerRoute(
    this IEndpointRouteBuilder endpoints,
    string pattern,
    Type controller,
    string action)
  {
    endpoints.MapAreaControllerRoute(
      areaName: $"{nameof(Enms)}.{nameof(Server)}",
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
