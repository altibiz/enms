using Enms.Business.Controllers;

namespace Enms.Business.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsBusiness(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints)
  {
    endpoints.MapAreaControllerRoute(
      "iot/push/{id}",
      typeof(IotController),
      nameof(IotController.Push)
    );

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
      new
      {
        controller = controller.Name.Remove(
          nameof(Microsoft.AspNetCore.Mvc.Controller).Length),
        action
      }
    );
  }
}
