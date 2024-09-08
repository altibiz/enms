using Enms.Business.Models.Enums;

namespace Enms.Client.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NavigationAttribute : Attribute
{
  public int Order { get; set; }

  public string? Title { get; set; }

  public bool IsVisible { get; set; } = true;

  public required string RouteValue { get; set; }

  public string Icon { get; set; } = "home";

  public Type[] Parents { get; set; } = Array.Empty<Type>();

  public RoleModel[] Allows { get; set; } = Array.Empty<RoleModel>();

  public RoleModel[] Disallows { get; set; } = Array.Empty<RoleModel>();
}
