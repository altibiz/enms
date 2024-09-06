using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public class EnmsOwningLayoutComponentBase : EnmsOwningComponentBase
{
  [Parameter]
  public RenderFragment? Body { get; set; }

  [DynamicDependency(
    DynamicallyAccessedMemberTypes.All,
    typeof(EnmsLayoutComponentBase))]
  public override Task SetParametersAsync(ParameterView parameters)
  {
    return base.SetParametersAsync(parameters);
  }
}