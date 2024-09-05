using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Enms.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_egauge_aggregates_lines_line_id_meter_id",
                table: "egauge_aggregates");

            migrationBuilder.DropForeignKey(
                name: "fk_egauge_aggregates_meters_meter_id",
                table: "egauge_aggregates");

            migrationBuilder.DropForeignKey(
                name: "fk_egauge_measurements_lines_line_id_meter_id",
                table: "egauge_measurements");

            migrationBuilder.DropForeignKey(
                name: "fk_egauge_measurements_meters_meter_id",
                table: "egauge_measurements");

            migrationBuilder.DropForeignKey(
                name: "fk_events_representatives_representative_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_measurement_validators__measurement_validator_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_meters_meter_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_representatives_created_by_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_representatives_deleted_by_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_representatives_last_updated_by_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_measurement_validators_representatives_created_by_id",
                table: "measurement_validators");

            migrationBuilder.DropForeignKey(
                name: "fk_measurement_validators_representatives_deleted_by_id",
                table: "measurement_validators");

            migrationBuilder.DropForeignKey(
                name: "fk_measurement_validators_representatives_last_updated_by_id",
                table: "measurement_validators");

            migrationBuilder.DropForeignKey(
                name: "fk_meters_representatives_created_by_id",
                table: "meters");

            migrationBuilder.DropForeignKey(
                name: "fk_meters_representatives_deleted_by_id",
                table: "meters");

            migrationBuilder.DropForeignKey(
                name: "fk_meters_representatives_last_updated_by_id",
                table: "meters");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_representatives_created_by_id",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_representatives_deleted_by_id",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_representatives_last_updated_by_id",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "address",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "city",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "social_security_number",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "description",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "representatives",
                newName: "physical_person_phone_number");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "representatives",
                newName: "physical_person_name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "representatives",
                newName: "physical_person_email");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:audit_entity", "query,creation,modification,deletion")
                .Annotation("Npgsql:Enum:category_entity", "all,meter,meter_push")
                .Annotation("Npgsql:Enum:interval_entity", "quarter_hour,day,month")
                .Annotation("Npgsql:Enum:level_entity", "trace,debug,info,warning,error,critical")
                .Annotation("Npgsql:Enum:phase_entity", "l1,l2,l3")
                .Annotation("Npgsql:Enum:role_entity", "operator_representative,user_representative")
                .Annotation("Npgsql:Enum:topic_entity", "all,meter,meter_inactivity")
                .Annotation("Npgsql:PostgresExtension:timescaledb", ",,")
                .OldAnnotation("Npgsql:Enum:audit_entity", "query,creation,modification,deletion")
                .OldAnnotation("Npgsql:Enum:interval_entity", "quarter_hour,day,month")
                .OldAnnotation("Npgsql:Enum:level_entity", "trace,debug,info,warning,error,critical")
                .OldAnnotation("Npgsql:Enum:phase_entity", "l1,l2,l3")
                .OldAnnotation("Npgsql:Enum:role_entity", "operator_representative,user_representative")
                .OldAnnotation("Npgsql:PostgresExtension:timescaledb", ",,");

            migrationBuilder.AddColumn<int[]>(
                name: "topics",
                table: "representatives",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int>(
                name: "inactivity_duration_duration",
                table: "meters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "inactivity_duration_multiplier",
                table: "meters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int[]>(
                name: "categories",
                table: "events",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "content",
                table: "events",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "meter_id",
                table: "events",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    event_id = table.Column<long>(type: "bigint", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    topics = table.Column<int[]>(type: "integer[]", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    kind = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    resolved_by_id = table.Column<string>(type: "text", nullable: true),
                    resolved_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    meter_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_events__event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notifications_meters_meter_id",
                        column: x => x.meter_id,
                        principalTable: "meters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notifications_representatives_resolved_by_id",
                        column: x => x.resolved_by_id,
                        principalTable: "representatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notification_recipient_entity",
                columns: table => new
                {
                    recipient_id = table.Column<string>(type: "text", nullable: false),
                    notification_id = table.Column<long>(type: "bigint", nullable: false),
                    seen_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_recipient_entity", x => new { x.recipient_id, x.notification_id });
                    table.ForeignKey(
                        name: "fk_notification_recipient_entity_notification_entity_notificat",
                        column: x => x.notification_id,
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notification_recipient_entity_representatives_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "representatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_meter_id",
                table: "events",
                column: "meter_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_recipient_entity__notification_id",
                table: "notification_recipient_entity",
                column: "notification_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications__event_id",
                table: "notifications",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_meter_id",
                table: "notifications",
                column: "meter_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_resolved_by_id",
                table: "notifications",
                column: "resolved_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_aggregates_lines_line_id_meter_id",
                table: "egauge_aggregates",
                columns: new[] { "line_id", "meter_id" },
                principalTable: "lines",
                principalColumns: new[] { "line_id", "meter_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_aggregates_meters_meter_id",
                table: "egauge_aggregates",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_measurements_lines_line_id_meter_id",
                table: "egauge_measurements",
                columns: new[] { "line_id", "meter_id" },
                principalTable: "lines",
                principalColumns: new[] { "line_id", "meter_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_measurements_meters_meter_id",
                table: "egauge_measurements",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_events_meters_meter_id",
                table: "events",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_events_representatives_representative_id",
                table: "events",
                column: "representative_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_measurement_validators__measurement_validator_id",
                table: "lines",
                column: "measurement_validator_id",
                principalTable: "measurement_validators",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_meters_meter_id",
                table: "lines",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_representatives_created_by_id",
                table: "lines",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_representatives_deleted_by_id",
                table: "lines",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_representatives_last_updated_by_id",
                table: "lines",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_measurement_validators_representatives_created_by_id",
                table: "measurement_validators",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_measurement_validators_representatives_deleted_by_id",
                table: "measurement_validators",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_measurement_validators_representatives_last_updated_by_id",
                table: "measurement_validators",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_meters_representatives_created_by_id",
                table: "meters",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_meters_representatives_deleted_by_id",
                table: "meters",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_meters_representatives_last_updated_by_id",
                table: "meters",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_representatives_created_by_id",
                table: "representatives",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_representatives_deleted_by_id",
                table: "representatives",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_representatives_last_updated_by_id",
                table: "representatives",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_egauge_aggregates_lines_line_id_meter_id",
                table: "egauge_aggregates");

            migrationBuilder.DropForeignKey(
                name: "fk_egauge_aggregates_meters_meter_id",
                table: "egauge_aggregates");

            migrationBuilder.DropForeignKey(
                name: "fk_egauge_measurements_lines_line_id_meter_id",
                table: "egauge_measurements");

            migrationBuilder.DropForeignKey(
                name: "fk_egauge_measurements_meters_meter_id",
                table: "egauge_measurements");

            migrationBuilder.DropForeignKey(
                name: "fk_events_meters_meter_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "fk_events_representatives_representative_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_measurement_validators__measurement_validator_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_meters_meter_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_representatives_created_by_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_representatives_deleted_by_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_lines_representatives_last_updated_by_id",
                table: "lines");

            migrationBuilder.DropForeignKey(
                name: "fk_measurement_validators_representatives_created_by_id",
                table: "measurement_validators");

            migrationBuilder.DropForeignKey(
                name: "fk_measurement_validators_representatives_deleted_by_id",
                table: "measurement_validators");

            migrationBuilder.DropForeignKey(
                name: "fk_measurement_validators_representatives_last_updated_by_id",
                table: "measurement_validators");

            migrationBuilder.DropForeignKey(
                name: "fk_meters_representatives_created_by_id",
                table: "meters");

            migrationBuilder.DropForeignKey(
                name: "fk_meters_representatives_deleted_by_id",
                table: "meters");

            migrationBuilder.DropForeignKey(
                name: "fk_meters_representatives_last_updated_by_id",
                table: "meters");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_representatives_created_by_id",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_representatives_deleted_by_id",
                table: "representatives");

            migrationBuilder.DropForeignKey(
                name: "fk_representatives_representatives_last_updated_by_id",
                table: "representatives");

            migrationBuilder.DropTable(
                name: "notification_recipient_entity");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropIndex(
                name: "ix_events_meter_id",
                table: "events");

            migrationBuilder.DropColumn(
                name: "topics",
                table: "representatives");

            migrationBuilder.DropColumn(
                name: "inactivity_duration_duration",
                table: "meters");

            migrationBuilder.DropColumn(
                name: "inactivity_duration_multiplier",
                table: "meters");

            migrationBuilder.DropColumn(
                name: "categories",
                table: "events");

            migrationBuilder.DropColumn(
                name: "content",
                table: "events");

            migrationBuilder.DropColumn(
                name: "meter_id",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "physical_person_phone_number",
                table: "representatives",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "physical_person_name",
                table: "representatives",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "physical_person_email",
                table: "representatives",
                newName: "email");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:audit_entity", "query,creation,modification,deletion")
                .Annotation("Npgsql:Enum:interval_entity", "quarter_hour,day,month")
                .Annotation("Npgsql:Enum:level_entity", "trace,debug,info,warning,error,critical")
                .Annotation("Npgsql:Enum:phase_entity", "l1,l2,l3")
                .Annotation("Npgsql:Enum:role_entity", "operator_representative,user_representative")
                .Annotation("Npgsql:PostgresExtension:timescaledb", ",,")
                .OldAnnotation("Npgsql:Enum:audit_entity", "query,creation,modification,deletion")
                .OldAnnotation("Npgsql:Enum:category_entity", "all,meter,meter_push")
                .OldAnnotation("Npgsql:Enum:interval_entity", "quarter_hour,day,month")
                .OldAnnotation("Npgsql:Enum:level_entity", "trace,debug,info,warning,error,critical")
                .OldAnnotation("Npgsql:Enum:phase_entity", "l1,l2,l3")
                .OldAnnotation("Npgsql:Enum:role_entity", "operator_representative,user_representative")
                .OldAnnotation("Npgsql:Enum:topic_entity", "all,meter,meter_inactivity")
                .OldAnnotation("Npgsql:PostgresExtension:timescaledb", ",,");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "representatives",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "representatives",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "representatives",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "social_security_number",
                table: "representatives",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_aggregates_lines_line_id_meter_id",
                table: "egauge_aggregates",
                columns: new[] { "line_id", "meter_id" },
                principalTable: "lines",
                principalColumns: new[] { "line_id", "meter_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_aggregates_meters_meter_id",
                table: "egauge_aggregates",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_measurements_lines_line_id_meter_id",
                table: "egauge_measurements",
                columns: new[] { "line_id", "meter_id" },
                principalTable: "lines",
                principalColumns: new[] { "line_id", "meter_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_egauge_measurements_meters_meter_id",
                table: "egauge_measurements",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_events_representatives_representative_id",
                table: "events",
                column: "representative_id",
                principalTable: "representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_measurement_validators__measurement_validator_id",
                table: "lines",
                column: "measurement_validator_id",
                principalTable: "measurement_validators",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_meters_meter_id",
                table: "lines",
                column: "meter_id",
                principalTable: "meters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lines_representatives_created_by_id",
                table: "lines",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lines_representatives_deleted_by_id",
                table: "lines",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lines_representatives_last_updated_by_id",
                table: "lines",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_measurement_validators_representatives_created_by_id",
                table: "measurement_validators",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_measurement_validators_representatives_deleted_by_id",
                table: "measurement_validators",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_measurement_validators_representatives_last_updated_by_id",
                table: "measurement_validators",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_meters_representatives_created_by_id",
                table: "meters",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_meters_representatives_deleted_by_id",
                table: "meters",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_meters_representatives_last_updated_by_id",
                table: "meters",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_representatives_created_by_id",
                table: "representatives",
                column: "created_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_representatives_deleted_by_id",
                table: "representatives",
                column: "deleted_by_id",
                principalTable: "representatives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_representatives_representatives_last_updated_by_id",
                table: "representatives",
                column: "last_updated_by_id",
                principalTable: "representatives",
                principalColumn: "id");
        }
    }
}
