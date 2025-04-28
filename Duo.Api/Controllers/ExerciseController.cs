using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IRepository repository;

        public ExerciseController(IRepository repository)
        {
            this.repository = repository;
        }

        private List<Exercise> MergeExercises(List<Exercise> exercises)
        {
            var mergedExercises = new List<Exercise>();
            var exerciseMap = new Dictionary<int, Exercise>();

            foreach (var exercise in exercises)
            {
                if (!exerciseMap.TryGetValue(exercise.ExerciseId, out var existingExercise))
                {
                    // First occurrence of this exercise ID
                    exerciseMap[exercise.ExerciseId] = exercise switch
                    {
                        MultipleChoiceExercise mc => new MultipleChoiceExercise(mc.ExerciseId, mc.Question, mc.Difficulty, new List<MultipleChoiceAnswerModel>(mc.Choices)),
                        FillInTheBlankExercise fb => new FillInTheBlankExercise(fb.ExerciseId, fb.Question, fb.Difficulty, new List<string>(fb.PossibleCorrectAnswers)),
                        AssociationExercise assoc => new AssociationExercise(assoc.ExerciseId, assoc.Question, assoc.Difficulty, new List<string>(assoc.FirstAnswersList), new List<string>(assoc.SecondAnswersList)),
                        FlashcardExercise flash => new FlashcardExercise(flash.ExerciseId, flash.Question, flash.Answer, flash.Difficulty),
                        _ => exercise // Keep other types unchanged
                    };
                }
                else
                {
                    // Merge the data
                    switch (existingExercise)
                    {
                        case MultipleChoiceExercise existingMC when exercise is MultipleChoiceExercise newMC:
                            // remove from the choicesthe correct choice, as it was previously added
                            newMC.Choices.RemoveAll(c => c.IsCorrect);
                            existingMC.Choices.AddRange(newMC.Choices);
                            break;

                        case FillInTheBlankExercise existingFB when exercise is FillInTheBlankExercise newFB:
                            existingFB.PossibleCorrectAnswers.AddRange(newFB.PossibleCorrectAnswers);
                            break;

                        case AssociationExercise existingAssoc when exercise is AssociationExercise newAssoc:
                            existingAssoc.FirstAnswersList.AddRange(newAssoc.FirstAnswersList);
                            existingAssoc.SecondAnswersList.AddRange(newAssoc.SecondAnswersList);
                            break;
                    }
                }
            }

            // Add the merged multiple-choice exercises to the final list
            mergedExercises.AddRange(exerciseMap.Values);

            return mergedExercises;
        }

        [HttpGet]
        public async Task<ActionResult<List<Exercise>>> GetAllExercisesAsync()
        {
            try
            {
                var exercises = await repository.GetExercisesFromDbAsync();
                var mergedExercises = MergeExercises(exercises);
                return Ok(mergedExercises); // OK 200 with the list of merged exercises
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Exercise>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Exercise ID must be greater than 0.");
            }

            try
            {
                var exercise = await repository.GetExerciseByIdAsync(id); // assuming you updated repository to EFCore
                List<Exercise> exercises = [exercise];
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

            var exercises = quiz.Exercises?.ToList() ?? new List<Exercise>();
            return Ok(exercises);
        }

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

            var exercises = exam.Exercises?.ToList() ?? new List<Exercise>();
            return Ok(exercises);
        }

        [HttpPost]
        public async Task<ActionResult> AddExercise([FromBody] Exercise exercise)
        {
            if (exercise == null)
            {
                return BadRequest("Exercise cannot be null.");
            }

            await repository.AddExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = exercise.ExerciseId }, exercise);
        }

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
    }
}
