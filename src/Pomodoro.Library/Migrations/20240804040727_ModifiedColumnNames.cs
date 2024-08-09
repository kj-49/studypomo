using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.Library.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "Course",
                newName: "DateCreated");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Course",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Course",
                newName: "Updated");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Course",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
