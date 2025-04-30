using System.Diagnostics.CodeAnalysis;
using Duo.Api.DTO.Requests;
using Duo.Api.Models.Exercises;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing exercises, including CRUD operations and retrieval by quiz or exam.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExerciseController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [Route("api/[controller]")]
    [ExcludeFromCodeCoverage]
    public class ExerciseController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Retrieves all exercises from the database.
        /// </summary>
        /// <returns>A list of all exercises.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Exercise>>> GetAllExercisesAsync()
        {
            try
            {
                var exercises = await repository.GetExercisesFromDbAsync();
                var mergedExercises = MergeExercises(exercises);
                return Ok(mergedExercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an exercise by its ID.
        /// </summary>
        /// <param name="id">The ID of the exercise to retrieve.</param>
        /// <returns>The exercise if found; otherwise, NotFound.</returns>
        [HttpGet("{id}", Name = "GetExerciseById")]
        public async Task<ActionResult<Exercise>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Exercise ID must be greater than 0.");
            }

            try
            {
                var exercise = await repository.GetExerciseByIdAsync(id);
                var exercises = new List<Exercise> { exercise };
                var mergedExercises = MergeExercises(exercises);

                if (mergedExercises == null || mergedExercises.Count == 0)
                {
                    return NotFound($"Exercise with ID {id} not found.");
                }

                return Ok(mergedExercises[0]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving exercise with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all exercises associated with a specific quiz.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <returns>A list of exercises associated with the quiz.</returns>
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<List<Exercise>>> GetQuizExercises(int quizId)
        {
            if (quizId <= 0)
            {
                return BadRequest("Invalid quiz ID.");
            }

            var quiz = await repository.GetQuizByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            var exercises = quiz.Exercises?.ToList() ?? [];
            return Ok(exercises);
        }

        /// <summary>
        /// Retrieves all exercises associated with a specific exam.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>A list of exercises associated with the exam.</returns>
        [HttpGet("exam/{examId}")]
        public async Task<ActionResult<List<Exercise>>> GetExamExercises(int examId)
        {
            if (examId <= 0)
            {
                return BadRequest("Invalid exam ID.");
            }

            var exam = await repository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            var exercises = exam.Exercises?.ToList() ?? [];
            return Ok(exercises);
        }

        /// <summary>
        /// Adds a new exercise to the database.
        /// </summary>
        /// <param name="exercise">The exercise to add.</param>
        /// <returns>The created exercise.</returns>
        [HttpPost]
        public async Task<ActionResult> AddExercise([FromBody] CreateExerciseDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid payload.");
            }

            // Map DTO → concrete Exercise
            var exercise = new MultipleChoiceExercise
            {
                Question = dto.Question,
                Difficulty = dto.Difficulty,
                // link up Exam/Quiz associations here if needed
            };

            await repository.AddExerciseAsync(exercise);
            return CreatedAtRoute(
              routeName: "GetExerciseById",
              routeValues: new { id = exercise.ExerciseId },
              value: exercise);
        }

        /// <summary>
        /// Deletes an exercise by its ID.
        /// </summary>
        /// <param name="id">The ID of the exercise to delete.</param>
        /// <returns>NoContent if deleted; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExercise(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid exercise ID.");
            }

            var existingExercise = await repository.GetExerciseByIdAsync(id);
            if (existingExercise == null)
            {
                return NotFound();
            }

            await repository.DeleteExerciseAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Merges duplicate exercises into a single instance.
        /// </summary>
        /// <param name="exercises">The list of exercises to merge.</param>
        /// <returns>A list of merged exercises.</returns>
        private static List<Exercise> MergeExercises(List<Exercise> exercises)
        {
            var mergedExercises = new List<Exercise>();
            var exerciseMap = new Dictionary<int, Exercise>();

            foreach (var exercise in exercises)
            {
                if (!exerciseMap.TryGetValue(exercise.ExerciseId, out var existingExercise))
                {
                    exerciseMap[exercise.ExerciseId] = exercise switch
                    {
                        MultipleChoiceExercise mc => new MultipleChoiceExercise { ExerciseId = mc.ExerciseId, Question = mc.Question!, Difficulty = mc.Difficulty, Choices = [.. mc.Choices!] },
                        FillInTheBlankExercise fb => new FillInTheBlankExercise { ExerciseId = fb.ExerciseId, Question = fb.Question!, Difficulty = fb.Difficulty, PossibleCorrectAnswers = [.. fb.PossibleCorrectAnswers!] },
                        AssociationExercise assoc => new AssociationExercise { ExerciseId = assoc.ExerciseId, Question = assoc.Question!, Difficulty = assoc.Difficulty, FirstAnswersList = [.. assoc.FirstAnswersList], SecondAnswersList = [.. assoc.SecondAnswersList] },
                        FlashcardExercise flash => new FlashcardExercise { ExerciseId = flash.ExerciseId, Question = flash.Question!, Answer = flash.Answer, Difficulty = flash.Difficulty },
                        _ => exercise
                    };
                }
                else
                {
                    // Merge the data
                    switch (existingExercise)
                    {
                        case MultipleChoiceExercise existingMC when exercise is MultipleChoiceExercise newMC:
                            newMC.Choices!.RemoveAll(c => c.IsCorrect);
                            existingMC.Choices!.AddRange(newMC.Choices);
                            break;

                        case FillInTheBlankExercise existingFB when exercise is FillInTheBlankExercise newFB:
                            existingFB.PossibleCorrectAnswers!.AddRange(newFB.PossibleCorrectAnswers!);
                            break;

                        case AssociationExercise existingAssoc when exercise is AssociationExercise newAssoc:
                            existingAssoc.FirstAnswersList.AddRange(newAssoc.FirstAnswersList);
                            existingAssoc.SecondAnswersList.AddRange(newAssoc.SecondAnswersList);
                            break;
                    }
                }
            }

            mergedExercises.AddRange(exerciseMap.Values);

            return mergedExercises;
        }

        #endregion
    }
}