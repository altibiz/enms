using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Enms.Data.Migrations
{
  /// <inheritdoc />
  public partial class Initial : Migration
  {
    private static readonly string[] columns = new[] { "auditable_entity_table", "auditable_entity_id" };
    private static readonly string[] columnsArray = new[] { "auditable_entity_type", "auditable_entity_id" };

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterDatabase()
          .Annotation("Npgsql:Enum:audit_entity", "query,creation,modification,deletion")
          .Annotation("Npgsql:Enum:interval_entity", "quarter_hour,day,month")
          .Annotation("Npgsql:Enum:level_entity", "trace,debug,info,warning,error,critical")
          .Annotation("Npgsql:Enum:phase_entity", "l1,l2,l3")
          .Annotation("Npgsql:Enum:role_entity", "operator_representative,user_representative")
          .Annotation("Npgsql:PostgresExtension:timescaledb", ",,");

      migrationBuilder.CreateTable(
          name: "representatives",
          columns: table => new
          {
            id = table.Column<string>(type: "text", nullable: false),
            role = table.Column<int>(type: "integer", nullable: false),
            name = table.Column<string>(type: "text", nullable: false),
            social_security_number = table.Column<string>(type: "text", nullable: false),
            address = table.Column<string>(type: "text", nullable: false),
            postal_code = table.Column<string>(type: "text", nullable: false),
            city = table.Column<string>(type: "text", nullable: false),
            email = table.Column<string>(type: "text", nullable: false),
            phone_number = table.Column<string>(type: "text", nullable: false),
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
            table.PrimaryKey("pk_representatives", x => x.id);
            table.ForeignKey(
                      name: "fk_representatives_representatives_created_by_id",
                      column: x => x.created_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "fk_representatives_representatives_deleted_by_id",
                      column: x => x.deleted_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "fk_representatives_representatives_last_updated_by_id",
                      column: x => x.last_updated_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
          });

      migrationBuilder.CreateTable(
          name: "events",
          columns: table => new
          {
            id = table.Column<long>(type: "bigint", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
            timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            level = table.Column<int>(type: "integer", nullable: false),
            description = table.Column<string>(type: "text", nullable: false),
            title = table.Column<string>(type: "text", nullable: false),
            kind = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
            auditable_entity_id = table.Column<string>(type: "text", nullable: true),
            auditable_entity_type = table.Column<string>(type: "text", nullable: true),
            auditable_entity_table = table.Column<string>(type: "text", nullable: true),
            audit = table.Column<int>(type: "integer", nullable: true),
            representative_id = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_events", x => x.id);
            table.ForeignKey(
                      name: "fk_events_representatives_representative_id",
                      column: x => x.representative_id,
                      principalTable: "representatives",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "measurement_validators",
          columns: table => new
          {
            id = table.Column<long>(type: "bigint", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
            kind = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
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
            table.PrimaryKey("pk_measurement_validators", x => x.id);
            table.ForeignKey(
                      name: "fk_measurement_validators_representatives_created_by_id",
                      column: x => x.created_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "fk_measurement_validators_representatives_deleted_by_id",
                      column: x => x.deleted_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "fk_measurement_validators_representatives_last_updated_by_id",
                      column: x => x.last_updated_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
          });

      migrationBuilder.CreateTable(
          name: "meters",
          columns: table => new
          {
            id = table.Column<string>(type: "text", nullable: false),
            messenger_id = table.Column<string>(type: "text", nullable: false),
            connection_power_w = table.Column<float>(type: "real", nullable: false),
            phases = table.Column<int[]>(type: "integer[]", nullable: false),
            kind = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
            measurement_validator_id = table.Column<long>(type: "bigint", nullable: true),
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
            table.PrimaryKey("pk_meters", x => x.id);
            table.ForeignKey(
                      name: "fk_meters_measurement_validators__measurement_validator_id",
                      column: x => x.measurement_validator_id,
                      principalTable: "measurement_validators",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "fk_meters_representatives_created_by_id",
                      column: x => x.created_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "fk_meters_representatives_deleted_by_id",
                      column: x => x.deleted_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "fk_meters_representatives_last_updated_by_id",
                      column: x => x.last_updated_by_id,
                      principalTable: "representatives",
                      principalColumn: "id");
          });

      migrationBuilder.CreateTable(
          name: "abb_b2x_aggregates",
          columns: table => new
          {
            timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            interval = table.Column<int>(type: "integer", nullable: false),
            meter_id = table.Column<string>(type: "text", nullable: false),
            count = table.Column<long>(type: "bigint", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_abb_b2x_aggregates", x => new { x.timestamp, x.interval, x.meter_id });
            table.ForeignKey(
                      name: "fk_abb_b2x_aggregates_meters_meter_id",
                      column: x => x.meter_id,
                      principalTable: "meters",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          })
          .Annotation("TimescaleHypertable", "timestamp,meter_id:number_partitions => 2");

      migrationBuilder.CreateTable(
          name: "abb_b2x_measurements",
          columns: table => new
          {
            timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            meter_id = table.Column<string>(type: "text", nullable: false),
            voltage_v = table.Column<float>(type: "real", nullable: false),
            power_w = table.Column<float>(type: "real", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("pk_abb_b2x_measurements", x => new { x.timestamp, x.meter_id });
            table.ForeignKey(
                      name: "fk_abb_b2x_measurements_meters_meter_id",
                      column: x => x.meter_id,
                      principalTable: "meters",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Cascade);
          })
          .Annotation("TimescaleHypertable", "timestamp,meter_id:number_partitions => 2");

      migrationBuilder.CreateIndex(
          name: "ix_abb_b2x_aggregates_meter_id",
          table: "abb_b2x_aggregates",
          column: "meter_id");

      migrationBuilder.CreateIndex(
          name: "ix_abb_b2x_measurements_meter_id",
          table: "abb_b2x_measurements",
          column: "meter_id");

      migrationBuilder.CreateIndex(
          name: "ix_events_auditable_entity_table_auditable_entity_id",
          table: "events",
          columns: columns);

      migrationBuilder.CreateIndex(
          name: "ix_events_auditable_entity_type_auditable_entity_id",
          table: "events",
          columns: columnsArray);

      migrationBuilder.CreateIndex(
          name: "ix_events_representative_id",
          table: "events",
          column: "representative_id");

      migrationBuilder.CreateIndex(
          name: "ix_measurement_validators_created_by_id",
          table: "measurement_validators",
          column: "created_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_measurement_validators_deleted_by_id",
          table: "measurement_validators",
          column: "deleted_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_measurement_validators_last_updated_by_id",
          table: "measurement_validators",
          column: "last_updated_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_meters__measurement_validator_id",
          table: "meters",
          column: "measurement_validator_id",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "ix_meters_created_by_id",
          table: "meters",
          column: "created_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_meters_deleted_by_id",
          table: "meters",
          column: "deleted_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_meters_last_updated_by_id",
          table: "meters",
          column: "last_updated_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_representatives_created_by_id",
          table: "representatives",
          column: "created_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_representatives_deleted_by_id",
          table: "representatives",
          column: "deleted_by_id");

      migrationBuilder.CreateIndex(
          name: "ix_representatives_last_updated_by_id",
          table: "representatives",
          column: "last_updated_by_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "abb_b2x_aggregates");

      migrationBuilder.DropTable(
          name: "abb_b2x_measurements");

      migrationBuilder.DropTable(
          name: "events");

      migrationBuilder.DropTable(
          name: "meters");

      migrationBuilder.DropTable(
          name: "measurement_validators");

      migrationBuilder.DropTable(
          name: "representatives");
    }
  }
}
