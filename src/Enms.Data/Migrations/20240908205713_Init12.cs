using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notification_recipient_entity_notification_entity_notificat",
                table: "notification_recipient_entity");

            migrationBuilder.DropForeignKey(
                name: "fk_notification_recipient_entity_representatives_recipient_id",
                table: "notification_recipient_entity");

            migrationBuilder.DropPrimaryKey(
                name: "pk_notification_recipient_entity",
                table: "notification_recipient_entity");

            migrationBuilder.RenameTable(
                name: "notification_recipient_entity",
                newName: "notification_recipients");

            migrationBuilder.RenameIndex(
                name: "ix_notification_recipient_entity__notification_id",
                table: "notification_recipients",
                newName: "ix_notification_recipients__notification_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_notification_recipients",
                table: "notification_recipients",
                columns: new[] { "recipient_id", "notification_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_notification_recipients_notifications_notification_id",
                table: "notification_recipients",
                column: "notification_id",
                principalTable: "notifications",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_notification_recipients_representatives_recipient_id",
                table: "notification_recipients",
                column: "recipient_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notification_recipients_notifications_notification_id",
                table: "notification_recipients");

            migrationBuilder.DropForeignKey(
                name: "fk_notification_recipients_representatives_recipient_id",
                table: "notification_recipients");

            migrationBuilder.DropPrimaryKey(
                name: "pk_notification_recipients",
                table: "notification_recipients");

            migrationBuilder.RenameTable(
                name: "notification_recipients",
                newName: "notification_recipient_entity");

            migrationBuilder.RenameIndex(
                name: "ix_notification_recipients__notification_id",
                table: "notification_recipient_entity",
                newName: "ix_notification_recipient_entity__notification_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_notification_recipient_entity",
                table: "notification_recipient_entity",
                columns: new[] { "recipient_id", "notification_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_notification_recipient_entity_notification_entity_notificat",
                table: "notification_recipient_entity",
                column: "notification_id",
                principalTable: "notifications",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_notification_recipient_entity_representatives_recipient_id",
                table: "notification_recipient_entity",
                column: "recipient_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
