using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Enms.Client.Base;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
public abstract class EnmsOwningComponentBase : EnmsComponentBase, IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
{
  private AsyncServiceScope? _scope;

  [Inject]
  private IServiceScopeFactory ScopeFactory { get; set; } = default!;

  protected bool IsDisposed { get; private set; }

  protected IServiceProvider ScopedServices
  {
    get
    {
      if (ScopeFactory == null)
      {
        throw new InvalidOperationException(
          "Services cannot be accessed before the component is initialized."
        );
      }

      ObjectDisposedException.ThrowIf(IsDisposed, this);
      if (!_scope.HasValue)
      {
        _scope = ScopeFactory.CreateAsyncScope();
      }

      return _scope.Value.ServiceProvider;
    }
  }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
  void IDisposable.Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
  {
    if (!IsDisposed)
    {
      _scope?.Dispose();
      _scope = null;
      Dispose(true);
      IsDisposed = true;
    }
  }

#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
  protected virtual void Dispose(bool disposing)
#pragma warning restore S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
  {
  }
}
