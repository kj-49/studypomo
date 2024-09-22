using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPomo.Library.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedStudySession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySession_StudyType_StudyTypeId",
                table: "StudySession");

            migrationBuilder.DropIndex(
                name: "IX_StudySession_StudyTypeId",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "Ended",
                table: "StudySession");

            migrationBuilder.RenameColumn(
                name: "StudyTypeId",
                table: "StudySession",
                newName: "TotalPomodoros");

            migrationBuilder.RenameColumn(
                name: "Started",
                table: "StudySession",
                newName: "DateStarted");

            migrationBuilder.AddColumn<string>(
                name: "SessionUUID",
                table: "StudySession",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "TotalBreakTime",
                table: "StudySession",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalFocusTime",
                table: "StudySession",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionUUID",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "TotalBreakTime",
                table: "StudySession");

            migrationBuilder.DropColumn(
                name: "TotalFocusTime",
                table: "StudySession");

            migrationBuilder.RenameColumn(
                name: "TotalPomodoros",
                table: "StudySession",
                newName: "StudyTypeId");

            migrationBuilder.RenameColumn(
                name: "DateStarted",
                table: "StudySession",
                newName: "Started");

            migrationBuilder.AddColumn<DateTime>(
                name: "Ended",
                table: "StudySession",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_StudySession_StudyTypeId",
                table: "StudySession",
                column: "StudyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySession_StudyType_StudyTypeId",
                table: "StudySession",
                column: "StudyTypeId",
                principalTable: "StudyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
