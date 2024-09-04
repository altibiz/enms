namespace Enms.Business.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsBusiness(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints)
  {
    endpoints.MapAreaControllerRoute(
      "Enms.Business.Controllers.Iot.Push",
      "Enms.Business",
      "/iot/push/{id}",
      new { controller = "Iot", action = "Push" }
    );

    return app;
  }
}
