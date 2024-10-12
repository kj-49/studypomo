using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPomo.Library.Migrations
{
    /// <inheritdoc />
    public partial class OnboardingFlagAddedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnboarded",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnboarded",
                table: "AspNetUsers");
        }
    }
}
