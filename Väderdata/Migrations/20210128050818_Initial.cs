using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Väderdata.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherViewData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AverageTemperature = table.Column<double>(type: "float", nullable: false),
                    AverageHumidity = table.Column<double>(type: "float", nullable: false),
                    MouldRisk = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Season = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherViewData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    WeatherViewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherData_WeatherViewData_WeatherViewId",
                        column: x => x.WeatherViewId,
                        principalTable: "WeatherViewData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_WeatherViewId",
                table: "WeatherData",
                column: "WeatherViewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherData");

            migrationBuilder.DropTable(
                name: "WeatherViewData");
        }
    }
}
