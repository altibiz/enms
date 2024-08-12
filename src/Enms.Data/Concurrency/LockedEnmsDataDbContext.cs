namespace Enms.Data.Concurrency;

public class EnmsDataDbContextMutex(EnmsDataDbContext context)
{
  private readonly SemaphoreSlim semaphore = new(1, 1);

  private readonly EnmsDataDbContext _context = context;

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
  public class EnmsDataDbContextLock(EnmsDataDbContextMutex mutex) : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
  {
    public EnmsDataDbContext Context => mutex._context;

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
      mutex.semaphore.Release();
    }
  }
}
