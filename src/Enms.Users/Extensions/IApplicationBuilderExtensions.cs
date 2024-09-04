namespace Enms.Users.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsUsers(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints
  )
  {
    return app;
  }
}
