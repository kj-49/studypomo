using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomodoroLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedPreferredTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredTheme",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredTheme",
                table: "AspNetUsers");
        }
    }
}
