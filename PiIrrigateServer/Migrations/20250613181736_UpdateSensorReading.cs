using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiIrrigateServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSensorReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "SensorReadings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "SensorReadings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WaterFlow",
                table: "SensorReadings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WaterTemperature",
                table: "SensorReadings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "SensorReadings");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "SensorReadings");

            migrationBuilder.DropColumn(
                name: "WaterFlow",
                table: "SensorReadings");

            migrationBuilder.DropColumn(
                name: "WaterTemperature",
                table: "SensorReadings");
        }
    }
}
