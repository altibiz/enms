using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_lines__measurement_validator_id",
                table: "lines");

            migrationBuilder.CreateIndex(
                name: "ix_lines__measurement_validator_id",
                table: "lines",
                column: "measurement_validator_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_lines__measurement_validator_id",
                table: "lines");

            migrationBuilder.CreateIndex(
                name: "ix_lines__measurement_validator_id",
                table: "lines",
                column: "measurement_validator_id",
                unique: true);
        }
    }
}
