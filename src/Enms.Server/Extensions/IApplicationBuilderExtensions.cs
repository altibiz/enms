namespace Enms.Server.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsServer(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints
  )
  {
    return app;
  }
}
