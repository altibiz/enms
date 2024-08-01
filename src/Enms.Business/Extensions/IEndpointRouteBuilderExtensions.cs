using System.Reflection;

namespace Enms.Business.Extensions;

public static class IEndpointRouteBuilderExtensions
{
  public static IEndpointRouteBuilder MapEnmsIot(
    this IEndpointRouteBuilder endpoints,
    string controller,
    string action,
    string prefix
  )
  {
    endpoints.MapAreaControllerRoute(
      "Enms.Business.Ios",
      Assembly.GetCallingAssembly().GetName().Name
      ?? throw new InvalidOperationException("Assembly name not found"),
      prefix + "/{id}",
      new { controller, action }
    );

    return endpoints;
  }
}
