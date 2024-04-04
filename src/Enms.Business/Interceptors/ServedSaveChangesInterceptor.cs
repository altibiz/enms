using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enms.Business.Interceptors;

public class ServedSaveChangesInterceptor : SaveChangesInterceptor
{
  protected readonly IServiceProvider _serviceProvider;

  public ServedSaveChangesInterceptor(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }
}
