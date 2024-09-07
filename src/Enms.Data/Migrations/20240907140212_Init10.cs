using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "max_max_inactivity_period_multiplier",
                table: "meters",
                newName: "max_inactivity_period_multiplier");

            migrationBuilder.RenameColumn(
                name: "max_max_inactivity_period_duration",
                table: "meters",
                newName: "max_inactivity_period_duration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_multiplier",
                table: "meters",
                newName: "max_max_inactivity_period_multiplier");

            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_duration",
                table: "meters",
                newName: "max_max_inactivity_period_duration");
        }
    }
}
