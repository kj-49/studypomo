using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPomo.Library.Migrations
{
    /// <inheritdoc />
    public partial class AddedDateModifiedToStudySession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudySession",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudySession");
        }
    }
}
