using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    ExerciseType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FirstAnswersList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondAnswersList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossibleCorrectAnswers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElapsedTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "ExamExercises",
                columns: table => new
                {
                    ExamsId = table.Column<int>(type: "int", nullable: false),
                    ExercisesExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamExercises", x => new { x.ExamsId, x.ExercisesExerciseId });
                    table.ForeignKey(
                        name: "FK_ExamExercises_Exams_ExamsId",
                        column: x => x.ExamsId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamExercises_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceAnswerModel",
                columns: table => new
                {
                    AnswerModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    MultipleChoiceExerciseExerciseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceAnswerModel", x => x.AnswerModelId);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceAnswerModel_Exercises_MultipleChoiceExerciseExerciseId",
                        column: x => x.MultipleChoiceExerciseExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId");
                });

            migrationBuilder.CreateTable(
                name: "QuizExercises",
                columns: table => new
                {
                    ExercisesExerciseId = table.Column<int>(type: "int", nullable: false),
                    QuizzesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizExercises", x => new { x.ExercisesExerciseId, x.QuizzesId });
                    table.ForeignKey(
                        name: "FK_QuizExercises_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizExercises_Quizzes_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamExercises_ExercisesExerciseId",
                table: "ExamExercises",
                column: "ExercisesExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_Question",
                table: "Exercises",
                column: "Question");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceAnswerModel_MultipleChoiceExerciseExerciseId",
                table: "MultipleChoiceAnswerModel",
                column: "MultipleChoiceExerciseExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizExercises_QuizzesId",
                table: "QuizExercises",
                column: "QuizzesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamExercises");

            migrationBuilder.DropTable(
                name: "MultipleChoiceAnswerModel");

            migrationBuilder.DropTable(
                name: "QuizExercises");

            migrationBuilder.DropTable(
                name: "Exercises");
        }
    }
}
