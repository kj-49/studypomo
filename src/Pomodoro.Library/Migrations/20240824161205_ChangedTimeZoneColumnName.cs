using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.Library.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTimeZoneColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IanaTimeZone",
                table: "AspNetUsers",
                newName: "TimeZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeZoneId",
                table: "AspNetUsers",
                newName: "IanaTimeZone");
        }
    }
}
