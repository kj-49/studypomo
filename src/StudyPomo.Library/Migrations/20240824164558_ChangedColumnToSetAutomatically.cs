using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPomo.Library.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnToSetAutomatically : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeZoneChosen",
                table: "AspNetUsers",
                newName: "SetTimeZoneAutomatically");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SetTimeZoneAutomatically",
                table: "AspNetUsers",
                newName: "TimeZoneChosen");
        }
    }
}
