using System.Collections.Generic;
using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "inactivity_duration_multiplier",
                table: "meters",
                newName: "max_inactivity_period_multiplier");

            migrationBuilder.RenameColumn(
                name: "inactivity_duration_duration",
                table: "meters",
                newName: "max_inactivity_period_duration");

            migrationBuilder.ConvertIntArrayToEnumArray<TopicEntity>("representatives", "topics");

            migrationBuilder.ConvertIntToEnum<RoleEntity>("representatives", "role");

            migrationBuilder.ConvertIntArrayToEnumArray<TopicEntity>("notifications", "topics");

            migrationBuilder.ConvertIntToEnum<DurationEntity>("meters", "max_inactivity_period_duration");

            migrationBuilder.ConvertIntArrayToEnumArray<PhaseEntity>("lines", "phases");

            migrationBuilder.ConvertIntToEnum<LevelEntity>("events", "level");

            migrationBuilder.ConvertIntArrayToEnumArray<CategoryEntity>("events", "categories");

            migrationBuilder.ConvertIntToEnum<AuditEntity>("events", "audit");

            migrationBuilder.ConvertIntToEnum<IntervalEntity>("egauge_aggregates", "interval");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_multiplier",
                table: "meters",
                newName: "inactivity_duration_multiplier");

            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_duration",
                table: "meters",
                newName: "inactivity_duration_duration");

            migrationBuilder.ConvertEnumArrayToIntArray<TopicEntity>("representatives", "topics");

            migrationBuilder.ConvertEnumToInt<RoleEntity>("representatives", "role");

            migrationBuilder.ConvertEnumArrayToIntArray<TopicEntity>("notifications", "topics");

            migrationBuilder.ConvertEnumToInt<DurationEntity>("meters", "max_inactivity_period_duration");

            migrationBuilder.ConvertEnumArrayToIntArray<PhaseEntity>("lines", "phases");

            migrationBuilder.ConvertEnumToInt<LevelEntity>("events", "level");

            migrationBuilder.ConvertEnumArrayToIntArray<CategoryEntity>("events", "categories");

            migrationBuilder.ConvertEnumToInt<AuditEntity>("events", "audit");

            migrationBuilder.ConvertEnumToInt<IntervalEntity>("egauge_aggregates", "interval");
        }
    }
}
