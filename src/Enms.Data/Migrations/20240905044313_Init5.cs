using System.Collections.Generic;
using Enms.Data.Entities.Enums;
using Enms.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable S3887
#pragma warning disable S2386
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

            EnumMappings.ApplyUpMigrations(migrationBuilder);

            migrationBuilder.Sql(@"
                ALTER TABLE egauge_aggregates
                ADD PRIMARY KEY (interval, timestamp, line_id, meter_id);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            EnumMappings.ApplyDownMigrations(migrationBuilder);

            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_multiplier",
                table: "meters",
                newName: "inactivity_duration_multiplier");

            migrationBuilder.RenameColumn(
                name: "max_inactivity_period_duration",
                table: "meters",
                newName: "inactivity_duration_duration");
        }

        public static class EnumMappings
        {
            public static readonly Dictionary<int, string> AuditEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "query" },
                { 1, "creation" },
                { 2, "modification" },
                { 3, "deletion" }
            };

            public static readonly Dictionary<string, int> AuditEntityEnumToInt = new Dictionary<string, int>
            {
                { "query", 0 },
                { "creation", 1 },
                { "modification", 2 },
                { "deletion", 3 }
            };

            public static readonly Dictionary<int, string> CategoryEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "all" },
                { 1, "meter" },
                { 2, "meter_push" }
            };

            public static readonly Dictionary<string, int> CategoryEntityEnumToInt = new Dictionary<string, int>
            {
                { "all", 0 },
                { "meter", 1 },
                { "meter_push", 2 }
            };

            public static readonly Dictionary<int, string> DurationEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "second" },
                { 1, "minute" },
                { 2, "hour" },
                { 3, "day" },
                { 4, "week" },
                { 5, "month" },
                { 6, "year" }
            };

            public static readonly Dictionary<string, int> DurationEntityEnumToInt = new Dictionary<string, int>
            {
                { "second", 0 },
                { "minute", 1 },
                { "hour", 2 },
                { "day", 3 },
                { "week", 4 },
                { "month", 5 },
                { "year", 6 }
            };

            public static readonly Dictionary<int, string> LevelEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "trace" },
                { 1, "debug" },
                { 2, "info" },
                { 3, "warning" },
                { 4, "error" },
                { 5, "critical" }
            };

            public static readonly Dictionary<string, int> LevelEntityEnumToInt = new Dictionary<string, int>
            {
                { "trace", 0 },
                { "debug", 1 },
                { "info", 2 },
                { "warning", 3 },
                { "error", 4 },
                { "critical", 5 }
            };

            public static readonly Dictionary<int, string> PhaseEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "l1" },
                { 1, "l2" },
                { 2, "l3" }
            };

            public static readonly Dictionary<string, int> PhaseEntityEnumToInt = new Dictionary<string, int>
            {
                { "l1", 0 },
                { "l2", 1 },
                { "l3", 2 }
            };

            public static readonly Dictionary<int, string> TopicEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "all" },
                { 1, "meter" },
                { 2, "meter_inactivity" }
            };

            public static readonly Dictionary<string, int> TopicEntityEnumToInt = new Dictionary<string, int>
            {
                { "all", 0 },
                { "meter", 1 },
                { "meter_inactivity", 2 }
            };

            public static readonly Dictionary<int, string> RoleEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "operator_representative" },
                { 1, "user_representative" }
            };

            public static readonly Dictionary<string, int> RoleEntityEnumToInt = new Dictionary<string, int>
            {
                { "operator_representative", 0 },
                { "user_representative", 1 }
            };

            public static readonly Dictionary<int, string> IntervalEntityIntToEnum = new Dictionary<int, string>
            {
                { 0, "quarter_hour" },
                { 1, "day" },
                { 2, "month" }
            };

            public static readonly Dictionary<string, int> IntervalEntityEnumToInt = new Dictionary<string, int>
            {
                { "quarter_hour", 0 },
                { "day", 1 },
                { "month", 2 }
            };

            public static void ApplyUpMigrations(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.ConvertIntArrayToEnumArray(
                    "representatives",
                    "topics",
                    EnumMappings.TopicEntityIntToEnum,
                    "topic_entity"
                );

                migrationBuilder.ConvertIntToEnum(
                    "representatives",
                    "role",
                    EnumMappings.RoleEntityIntToEnum,
                    "role_entity"
                );

                migrationBuilder.ConvertIntArrayToEnumArray(
                    "notifications",
                    "topics",
                    EnumMappings.TopicEntityIntToEnum,
                    "topic_entity"
                );

                migrationBuilder.ConvertIntToEnum(
                    "meters",
                    "max_inactivity_period_duration",
                    EnumMappings.DurationEntityIntToEnum,
                    "duration_entity"
                );

                migrationBuilder.ConvertIntArrayToEnumArray(
                    "lines",
                    "phases",
                    EnumMappings.PhaseEntityIntToEnum,
                    "phase_entity"
                );

                migrationBuilder.ConvertIntToEnum(
                    "events",
                    "level",
                    EnumMappings.LevelEntityIntToEnum,
                    "level_entity"
                );

                migrationBuilder.ConvertIntArrayToEnumArray(
                    "events",
                    "categories",
                    EnumMappings.CategoryEntityIntToEnum,
                    "category_entity"
                );

                migrationBuilder.ConvertIntToEnum(
                    "events",
                    "audit",
                    EnumMappings.AuditEntityIntToEnum,
                    "audit_entity"
                );

                migrationBuilder.ConvertIntToEnum(
                    "egauge_aggregates",
                    "interval",
                    EnumMappings.IntervalEntityIntToEnum,
                    "interval_entity"
                );
            }

            public static void ApplyDownMigrations(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.ConvertEnumArrayToIntArray(
                    "representatives",
                    "topics",
                    EnumMappings.TopicEntityEnumToInt
                );

                migrationBuilder.ConvertEnumToInt(
                    "representatives",
                    "role",
                    EnumMappings.RoleEntityEnumToInt
                );

                migrationBuilder.ConvertEnumArrayToIntArray(
                    "notifications",
                    "topics",
                    EnumMappings.TopicEntityEnumToInt
                );

                migrationBuilder.ConvertEnumToInt(
                    "meters",
                    "max_inactivity_period_duration",
                    EnumMappings.DurationEntityEnumToInt
                );

                migrationBuilder.ConvertEnumArrayToIntArray(
                    "lines",
                    "phases",
                    EnumMappings.PhaseEntityEnumToInt
                );

                migrationBuilder.ConvertEnumToInt(
                    "events",
                    "level",
                    EnumMappings.LevelEntityEnumToInt
                );

                migrationBuilder.ConvertEnumArrayToIntArray(
                    "events",
                    "categories",
                    EnumMappings.CategoryEntityEnumToInt
                );

                migrationBuilder.ConvertEnumToInt(
                    "events",
                    "audit",
                    EnumMappings.AuditEntityEnumToInt
                );

                migrationBuilder.ConvertEnumToInt(
                    "egauge_aggregates",
                    "interval",
                    EnumMappings.IntervalEntityEnumToInt
                );
            }
        }
    }
}
