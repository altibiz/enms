using Enms.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Enms.Data.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsData(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints)
  {
    using var scope = app.ApplicationServices.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    context.Database.Migrate();

    return app;
  }
}
