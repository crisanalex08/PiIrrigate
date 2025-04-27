using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiIrrigateServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedZones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Devices",
                newName: "ZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "Mac");

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    ZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.ZoneId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ZoneId",
                table: "Devices",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Zones_ZoneId",
                table: "Devices",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "ZoneId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Zones_ZoneId",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ZoneId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "ZoneId",
                table: "Devices",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "Id");
        }
    }
}
