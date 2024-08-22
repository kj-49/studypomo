using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.Library.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimeZoneFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IanaTimeZone",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "TimeZoneChosen",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IanaTimeZone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimeZoneChosen",
                table: "AspNetUsers");
        }
    }
}
