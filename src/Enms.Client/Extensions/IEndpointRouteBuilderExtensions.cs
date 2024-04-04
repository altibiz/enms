using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Enms.Client.Extensions;

public static class IEndpointRouteBuilderExtensions
{
  public static IEndpointRouteBuilder MapEnmsClient(
    this IEndpointRouteBuilder endpoints,
    string controller,
    string action,
    string prefix
  )
  {
    endpoints.MapAreaControllerRoute(
      "Enms.Client",
      Assembly.GetCallingAssembly().GetName().Name
      ?? throw new InvalidOperationException("Assembly name not found"),
      prefix + "/{**catchall}",
      new { controller, action }
    );
    endpoints.MapBlazorHub(prefix + "/_blazor");

    return endpoints;
  }
}
