using Microsoft.EntityFrameworkCore.Migrations;

namespace Väderdata.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherData_WeatherView_WeatherViewId",
                table: "WeatherData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherView",
                table: "WeatherView");

            migrationBuilder.RenameTable(
                name: "WeatherView",
                newName: "WeatherViewData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherViewData",
                table: "WeatherViewData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherData_WeatherViewData_WeatherViewId",
                table: "WeatherData",
                column: "WeatherViewId",
                principalTable: "WeatherViewData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherData_WeatherViewData_WeatherViewId",
                table: "WeatherData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherViewData",
                table: "WeatherViewData");

            migrationBuilder.RenameTable(
                name: "WeatherViewData",
                newName: "WeatherView");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherView",
                table: "WeatherView",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherData_WeatherView_WeatherViewId",
                table: "WeatherData",
                column: "WeatherViewId",
                principalTable: "WeatherView",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
