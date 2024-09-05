using Enms.Business.Controllers;

namespace Enms.Business.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsBusiness(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints)
  {
    endpoints.MapEnmsBusinessRoute(
      "/iot/push/{id}",
      typeof(IotController),
      nameof(IotController.Push)
    );

    return app;
  }

  private static void MapEnmsBusinessRoute(
    this IEndpointRouteBuilder endpoints,
    string pattern,
    Type controller,
    string action)
  {
    endpoints.MapAreaControllerRoute(
      areaName: $"{nameof(Enms)}.{nameof(Business)}",
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
