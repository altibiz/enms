namespace Enms.Email.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsEmail(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints
  )
  {
    return app;
  }
}
