using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceAnswerModel_Exercises_MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel");

            migrationBuilder.DropIndex(
                name: "IX_MultipleChoiceAnswerModel_MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel");

            migrationBuilder.DropColumn(
                name: "MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "MultipleChoiceAnswerModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "MultipleChoiceAnswerModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceAnswerModel_ExerciseId",
                table: "MultipleChoiceAnswerModel",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceAnswerModel_Exercises_ExerciseId",
                table: "MultipleChoiceAnswerModel",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceAnswerModel_Exercises_ExerciseId",
                table: "MultipleChoiceAnswerModel");

            migrationBuilder.DropIndex(
                name: "IX_MultipleChoiceAnswerModel_ExerciseId",
                table: "MultipleChoiceAnswerModel");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "MultipleChoiceAnswerModel");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "MultipleChoiceAnswerModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceAnswerModel_MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel",
                column: "MultipleChoiceExerciseExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceAnswerModel_Exercises_MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel",
                column: "MultipleChoiceExerciseExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId");
        }
    }
}
