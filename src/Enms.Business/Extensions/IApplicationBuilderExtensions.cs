namespace Enms.Business.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsBusiness(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints)
  {
    endpoints.MapAreaControllerRoute(
      "iot/push/{id}",
      typeof(Controllers.IotController),
      nameof(Controllers.IotController.Push)
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
