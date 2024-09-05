using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Enms.Data.Extensions;

public static class MigrationBuilderExtensions
{
  public static void ConvertIntToEnum<TEnum>(this MigrationBuilder migrationBuilder, string tableName, string columnName)
      where TEnum : Enum
  {
    var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
    var enumTypeName = typeof(TEnum).Name.ToSnakeCase();

    var sql = new StringBuilder();

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        ADD COLUMN temp_{columnName} {enumTypeName};
    ");

    sql.AppendLine($@"
        UPDATE {tableName}
        SET temp_{columnName} = CASE");

    foreach (var enumValue in enumValues)
    {
      var enumIndex = Convert.ToInt32(enumValue);
      var enumName = enumValue.ToString().ToSnakeCase();
      sql.AppendLine($@"
            WHEN {columnName} = {enumIndex} THEN '{enumName}'::{enumTypeName}");
    }

    sql.AppendLine($@"
        END;
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        DROP COLUMN {columnName};
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        RENAME COLUMN temp_{columnName} TO {columnName};
    ");

    migrationBuilder.Sql(sql.ToString());
  }

  public static void ConvertEnumToInt<TEnum>(this MigrationBuilder migrationBuilder, string tableName, string columnName)
      where TEnum : Enum
  {
    var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

    var sql = new StringBuilder();

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        ADD COLUMN temp_{columnName} integer;
    ");

    sql.AppendLine($@"
        UPDATE {tableName}
        SET temp_{columnName} = CASE");

    foreach (var enumValue in enumValues)
    {
      var enumIndex = Convert.ToInt32(enumValue);
      var enumName = enumValue.ToString().ToSnakeCase();
      sql.AppendLine($@"
            WHEN {columnName} = '{enumName}' THEN {enumIndex}");
    }

    sql.AppendLine($@"
        END;
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        DROP COLUMN {columnName};
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        RENAME COLUMN temp_{columnName} TO {columnName};
    ");

    migrationBuilder.Sql(sql.ToString());
  }

  public static void ConvertIntArrayToEnumArray<TEnum>(
    this MigrationBuilder migrationBuilder,
    string tableName,
    string columnName
  )
    where TEnum : Enum
  {
    var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
    var enumTypeName = typeof(TEnum).Name.ToSnakeCase();

    var sql = new StringBuilder();
    sql.AppendLine($@"
        ALTER TABLE {tableName}
        ADD COLUMN temp_{columnName} {enumTypeName}[];
    ");

    sql.AppendLine($@"
        UPDATE {tableName}
        SET temp_{columnName} = ARRAY(
            SELECT CASE");

    foreach (var enumValue in enumValues)
    {
      var enumIndex = Convert.ToInt32(enumValue);
      var enumName = enumValue.ToString().ToSnakeCase();
      sql.AppendLine($@"
        WHEN {columnName}_values = {enumIndex} THEN '{enumName}'::{enumTypeName}");
    }

    sql.AppendLine($@"
        END

        FROM unnest({columnName}) AS {columnName}_values
      );
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        DROP COLUMN {columnName};
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        RENAME COLUMN temp_{columnName} TO {columnName};
    ");

    migrationBuilder.Sql(sql.ToString());
  }

  public static void ConvertEnumArrayToIntArray<TEnum>(
      this MigrationBuilder migrationBuilder, string tableName, string columnName)
    where TEnum : Enum
  {
    var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

    var sql = new StringBuilder();
    sql.AppendLine($@"
        ALTER TABLE {tableName}
        ADD COLUMN temp_{columnName} integer[];
    ");

    sql.AppendLine($@"
        UPDATE {tableName}
        SET temp_{columnName} = ARRAY(
            SELECT CASE");

    foreach (var enumValue in enumValues)
    {
      var enumIndex = Convert.ToInt32(enumValue);
      var enumName = enumValue.ToString().ToSnakeCase();
      sql.AppendLine($@"
        WHEN {columnName}_values = '{enumName}' THEN {enumIndex}");
    }

    sql.AppendLine($@"
        END
        FROM unnest({columnName}) AS {columnName}_values
      );
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        DROP COLUMN {columnName};
    ");

    sql.AppendLine($@"
        ALTER TABLE {tableName}
        RENAME COLUMN temp_{columnName} TO {columnName};
    ");

    migrationBuilder.Sql(sql.ToString());
  }

  private static string ToSnakeCase(this string str)
  {
    return string.Concat(
        str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
        .ToLower();
  }
}
