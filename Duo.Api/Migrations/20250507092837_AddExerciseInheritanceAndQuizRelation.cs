using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseInheritanceAndQuizRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Exercises");
        }
    }
}
