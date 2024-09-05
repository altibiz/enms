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
    return app;
  }
}
