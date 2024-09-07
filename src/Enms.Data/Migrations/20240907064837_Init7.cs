using Enms.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_multiplier",
                table: "meters",
                newName: "push_delay_period_multiplier");

            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_duration",
                table: "meters",
                newName: "push_delay_period_duration");

            migrationBuilder.AddColumn<DurationEntity>(
                name: "max_max_inactivity_period_duration",
                table: "meters",
                type: "duration_entity",
                nullable: false,
                defaultValue: DurationEntity.Second);

            migrationBuilder.AddColumn<long>(
                name: "max_max_inactivity_period_multiplier",
                table: "meters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_max_inactivity_period_duration",
                table: "meters");

            migrationBuilder.DropColumn(
                name: "max_max_inactivity_period_multiplier",
                table: "meters");

            migrationBuilder.RenameColumn(
                name: "push_delay_period_multiplier",
                table: "meters",
                newName: "max_inactivity_period_multiplier");

            migrationBuilder.RenameColumn(
                name: "push_delay_period_duration",
                table: "meters",
                newName: "max_inactivity_period_duration");
        }
    }
}
