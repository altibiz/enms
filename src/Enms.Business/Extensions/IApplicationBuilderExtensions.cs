namespace Enms.Business.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsBusiness(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints)
  {
    return app;
  }
}
