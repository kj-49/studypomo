using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomodoroLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedLabelDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaskLabel",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaskLabel");
        }
    }
}
