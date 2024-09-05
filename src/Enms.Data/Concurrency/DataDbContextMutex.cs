using Enms.Data.Context;

namespace Enms.Data.Concurrency;

public class DataDbContextMutex(DataDbContext context)
{
  private readonly DataDbContext _context = context;
  private readonly SemaphoreSlim semaphore = new(1, 1);

  public EnmsDataDbContextLock Lock()
  {
    semaphore.Wait();
    return new EnmsDataDbContextLock(this);
  }

  public async ValueTask<EnmsDataDbContextLock> LockAsync()
  {
    await semaphore.WaitAsync();
    return new EnmsDataDbContextLock(this);
  }

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
  public class EnmsDataDbContextLock(DataDbContextMutex mutex) : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
  {
    public DataDbContext Context
    {
      get { return mutex._context; }
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
      mutex.semaphore.Release();
    }
  }
}
