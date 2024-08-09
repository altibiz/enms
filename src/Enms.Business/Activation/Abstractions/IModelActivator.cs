namespace Enms.Business.Activation.Abstractions;

public interface IModelActivator
{
  public Type ModelType { get; }

  public object Activate();
}
