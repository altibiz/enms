using Enms.Data.Entities.Enums;

namespace Enms.Business.Models;

public enum PhaseModel
{
  L1,
  L2,
  L3
}

public static class PhaseModelExtensions
{
  public static string ToTitle(this PhaseModel phase)
  {
    return phase switch
    {
      PhaseModel.L1 => "L1",
      PhaseModel.L2 => "L2",
      PhaseModel.L3 => "L3",
      _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
    };
  }

  public static string ToColor(this PhaseModel phase, int index = 0)
  {
    return phase switch
    {
      PhaseModel.L1 => Colors[index][0],
      PhaseModel.L2 => Colors[index][1],
      PhaseModel.L3 => Colors[index][2],
      _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
    };
  }

  public static PhaseModel ToModel(this PhaseEntity phase)
  {
    return phase switch
    {
      PhaseEntity.L1 => PhaseModel.L1,
      PhaseEntity.L2 => PhaseModel.L2,
      PhaseEntity.L3 => PhaseModel.L3,
      _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
    };
  }

  public static PhaseEntity ToEntity(this PhaseModel phase)
  {
    return phase switch
    {
      PhaseModel.L1 => PhaseEntity.L1,
      PhaseModel.L2 => PhaseEntity.L2,
      PhaseModel.L3 => PhaseEntity.L3,
      _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
    };
  }

  // NOTE: generate { |$i| if $i <= 10 { { out: (["#FB8C00", "#E91E63", "#20F97B"] | each { |x| pastel rotate ($i * 10) $x | pastel format hex }), next: ($i + 1) } } } 1 | to json
  private static readonly string[][] Colors = [
    [
      "#fbb400",
      "#e91e40",
      "#20f99f"
    ],
    [
      "#fbde00",
      "#e91e1e",
      "#20f9c3"
    ],
    [
      "#eefb00",
      "#e9401e",
      "#20f9e7"
    ],
    [
      "#c5fb00",
      "#e9621e",
      "#20e7f9"
    ],
    [
      "#9bfb00",
      "#e9841e",
      "#20c3f9"
    ],
    [
      "#71fb00",
      "#e9a51e",
      "#209ff9"
    ],
    [
      "#47fb00",
      "#e9c71e",
      "#207af9"
    ],
    [
      "#1dfb00",
      "#e9e91e",
      "#2056f9"
    ],
    [
      "#00fb0d",
      "#c7e91e",
      "#2032f9"
    ],
    [
      "#00fb36",
      "#a5e91e",
      "#3220f9"
    ]
  ];
}
