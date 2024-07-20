using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomodoroLibrary.Migrations
{
    /// <inheritdoc />
    public partial class MadePriorityNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyTask_TaskPriority_TaskPriorityId",
                table: "StudyTask");

            migrationBuilder.AlterColumn<int>(
                name: "TaskPriorityId",
                table: "StudyTask",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTask_TaskPriority_TaskPriorityId",
                table: "StudyTask",
                column: "TaskPriorityId",
                principalTable: "TaskPriority",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyTask_TaskPriority_TaskPriorityId",
                table: "StudyTask");

            migrationBuilder.AlterColumn<int>(
                name: "TaskPriorityId",
                table: "StudyTask",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTask_TaskPriority_TaskPriorityId",
                table: "StudyTask",
                column: "TaskPriorityId",
                principalTable: "TaskPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
