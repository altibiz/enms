using Enms.Data;
using Microsoft.EntityFrameworkCore;

namespace Enms.Business.Extensions;

public static class IApplicationBuilderExtensions
{
  public static IApplicationBuilder MigrateEnmsData(
    this IApplicationBuilder app)
  {
    using var scope = app.ApplicationServices.CreateScope();
    scope.ServiceProvider.GetRequiredService<EnmsDbContext>().Database
      .Migrate();
    return app;
  }
}
