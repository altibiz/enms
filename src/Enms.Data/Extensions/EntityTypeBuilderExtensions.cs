using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enms.Data.Extensions;

public static class EntityTypeBuilderExtensions
{
  public static EntityTypeBuilder HasTimescaleHypertable(
    this EntityTypeBuilder builder,
    string timeColumn,
    string? spaceColumn = null,
    string? spacePartitioning = null
  )
  {
    var value = timeColumn;
    if (spaceColumn is not null && spacePartitioning is not null)
    {
      value += $",{spaceColumn}:{spacePartitioning}";
    }

    builder.Metadata.AddAnnotation("TimescaleHypertable", value);

    return builder;
  }
}
