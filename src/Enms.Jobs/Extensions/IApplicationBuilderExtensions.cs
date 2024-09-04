using Enms.Jobs.Context;
using Microsoft.EntityFrameworkCore;

namespace Enms.Jobs.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder UseEnmsJobs(
    this IApplicationBuilder app,
    IEndpointRouteBuilder endpoints
  )
  {
    using var scope = app.ApplicationServices.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<JobsDbContext>();
    context.Database.Migrate();

    return app;
  }
}
