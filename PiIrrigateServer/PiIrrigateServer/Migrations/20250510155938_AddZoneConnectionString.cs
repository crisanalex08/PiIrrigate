using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiIrrigateServer.Migrations
{
    /// <inheritdoc />
    public partial class AddZoneConnectionString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                table: "Zones",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                table: "Zones");
        }
    }
}
