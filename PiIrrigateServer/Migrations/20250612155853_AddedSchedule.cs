using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PiIrrigateServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "C2DMethodCall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C2DMethodCall", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C2DMessageRequest",
                columns: table => new
                {
                    ZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    methodCallId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C2DMessageRequest", x => x.ZoneId);
                    table.ForeignKey(
                        name: "FK_C2DMessageRequest_C2DMethodCall_methodCallId",
                        column: x => x.methodCallId,
                        principalTable: "C2DMethodCall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    C2DMethodCallId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodParams_C2DMethodCall_C2DMethodCallId",
                        column: x => x.C2DMethodCallId,
                        principalTable: "C2DMethodCall",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Interval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AutoStop = table.Column<bool>(type: "boolean", nullable: false),
                    ScheduleStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_C2DMessageRequest_RequestZoneId",
                        column: x => x.RequestZoneId,
                        principalTable: "C2DMessageRequest",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_C2DMessageRequest_methodCallId",
                table: "C2DMessageRequest",
                column: "methodCallId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodParams_C2DMethodCallId",
                table: "MethodParams",
                column: "C2DMethodCallId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RequestZoneId",
                table: "Schedules",
                column: "RequestZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodParams");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "C2DMessageRequest");

            migrationBuilder.DropTable(
                name: "C2DMethodCall");
        }
    }
}
