using Enms.Business.Activation.Abstractions;

namespace Enms.Business.Activation.Base;

public abstract class ModelActivator<T> : IModelActivator
  where T : notnull
{
  public Type ModelType => typeof(T);

  public object Activate()
  {
    return ActivateConcrete();
  }

  public virtual T ActivateConcrete()
  {
    return Activator.CreateInstance<T>();
  }
}
