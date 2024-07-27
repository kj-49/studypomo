using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomodoroLibrary.Migrations
{
    /// <inheritdoc />
    public partial class DeadlineFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "StudyTask",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "StudyTask");
        }
    }
}
