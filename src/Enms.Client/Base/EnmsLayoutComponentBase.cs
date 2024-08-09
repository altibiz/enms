using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Enms.Business.Time;
using Microsoft.AspNetCore.Components;

namespace Enms.Client.Base;

public class EnmsLayoutComponentBase : EnmsComponentBase
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
