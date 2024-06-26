namespace Enms.Client.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NavigationAttribute : Attribute
{
  public string? Title { get; set; }

  public string? RouteValue { get; set; }

  public Type[] Parents { get; set; } = Array.Empty<Type>();
}
