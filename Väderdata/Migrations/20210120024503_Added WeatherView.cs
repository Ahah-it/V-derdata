using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Väderdata.Migrations
{
    public partial class AddedWeatherView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeatherViewId",
                table: "WeatherData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WeatherView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AverageTemperature = table.Column<double>(type: "float", nullable: false),
                    AverageHumidity = table.Column<double>(type: "float", nullable: false),
                    MouldRisk = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherView", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_WeatherViewId",
                table: "WeatherData",
                column: "WeatherViewId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherData_WeatherView_WeatherViewId",
                table: "WeatherData",
                column: "WeatherViewId",
                principalTable: "WeatherView",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherData_WeatherView_WeatherViewId",
                table: "WeatherData");

            migrationBuilder.DropTable(
                name: "WeatherView");

            migrationBuilder.DropIndex(
                name: "IX_WeatherData_WeatherViewId",
                table: "WeatherData");

            migrationBuilder.DropColumn(
                name: "WeatherViewId",
                table: "WeatherData");
        }
    }
}
