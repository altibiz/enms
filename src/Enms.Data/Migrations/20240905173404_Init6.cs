using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "network_user_id",
                table: "representatives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "owner_id",
                table: "lines",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "network_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    legal_person_address = table.Column<string>(type: "text", nullable: false),
                    legal_person_city = table.Column<string>(type: "text", nullable: false),
                    legal_person_email = table.Column<string>(type: "text", nullable: false),
                    legal_person_name = table.Column<string>(type: "text", nullable: false),
                    legal_person_phone_number = table.Column<string>(type: "text", nullable: false),
                    legal_person_postal_code = table.Column<string>(type: "text", nullable: false),
                    legal_person_social_security_number = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<string>(type: "text", nullable: true),
                    last_updated_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_updated_by_id = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by_id = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_network_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_network_users_representatives_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "representatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_network_users_representatives_deleted_by_id",
                        column: x => x.deleted_by_id,
                        principalTable: "representatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_network_users_representatives_last_updated_by_id",
                        column: x => x.last_updated_by_id,
                        principalTable: "representatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_representatives__network_user_id",
                table: "representatives",
                column: "network_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_lines__owner_id",
                table: "lines",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_network_users_created_by_id",
                table: "network_users",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_network_users_deleted_by_id",
                table: "network_users",
                column: "deleted_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_network_users_last_updated_by_id",
                table: "network_users",
                column: "last_updated_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_lines_network_users__owner_id",
                table: "lines",
                column: "owner_id",
                principalTable: "network_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_network_users_network_user_id",
                table: "representatives",
                column: "network_user_id",
                principalTable: "network_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lines_network_users__owner_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_network_users_network_user_id",
                table: "representatives");

            migrationBuilder.DropTable(
                name: "network_users");

            migrationBuilder.DropIndex(
                name: "ix_representatives__network_user_id",
                table: "representatives");

            migrationBuilder.DropIndex(
                name: "ix_lines__owner_id",
                table: "lines");

            migrationBuilder.DropColumn(
                name: "network_user_id",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "lines");
        }
    }
}
