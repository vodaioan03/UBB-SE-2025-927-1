using Microsoft.AspNetCore.Mvc;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using System.Threading.Tasks;
using Duo.Api.Models.Exercises; // needed for List<Exercise>

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("quiz")]
    public class QuizController : BaseController
    {
        public QuizController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Adds a new quiz to the database.
        /// </summary>
        /// <param name="quiz">The quiz data to add.</param>
        /// <returns>The added quiz.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddQuiz([FromForm] Quiz quiz)
        {
            await repository.AddQuizAsync(quiz);
            return Ok(quiz);
        }

        /// <summary>
        /// Retrieves a quiz by its ID.
        /// </summary>
        /// <param name="id">The ID of the quiz to retrieve.</param>
        /// <returns>The quiz if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetQuiz([FromQuery] int id)
        {
            var quiz = await repository.GetQuizByIdAsync(id);
            if (quiz == null)
                return NotFound();
            return Ok(quiz);
        }

        /// <summary>
        /// Lists all quizzes in the database.
        /// </summary>
        /// <returns>A list of all quizzes.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListQuizzes()
        {
            var quizzes = await repository.GetQuizzesFromDbAsync();
            return Ok(quizzes);
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        /// <param name="updatedQuiz">The updated quiz data, including Id.</param>
        /// <returns>The updated quiz if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuiz([FromForm] Quiz updatedQuiz)
        {
            var quiz = await repository.GetQuizByIdAsync(updatedQuiz.Id);
            if (quiz == null)
                return NotFound();
            await repository.UpdateQuizAsync(updatedQuiz);
            return Ok(updatedQuiz);
        }

        /// <summary>
        /// Deletes a quiz by its ID.
        /// </summary>
        /// <param name="id">The ID of the quiz to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQuiz([FromQuery] int id)
        {
            var quiz = await repository.GetQuizByIdAsync(id);
            if (quiz == null)
                return NotFound();
            await repository.DeleteQuizAsync(id);
            return Ok();
        }

        /// <summary>
        /// Retrieves all quizzes from a specific section.
        /// </summary>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>A list of quizzes from the section.</returns>
        [HttpGet("get-all-section")]
        public async Task<IActionResult> GetAllQuizzesFromSection([FromQuery] int sectionId)
        {
            var quizzes = await repository.GetAllQuizzesFromSectionAsync(sectionId);
            return Ok(quizzes);
        }

        /// <summary>
        /// Retrieves the number of quizzes in a specific section.
        /// </summary>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>The number of quizzes.</returns>
        [HttpGet("count-from-section")]
        public async Task<IActionResult> CountQuizzesFromSection([FromQuery] int sectionId)
        {
            var count = await repository.CountQuizzesFromSectionAsync(sectionId);
            return Ok(count);
        }

        /// <summary>
        /// Retrieves the last order number from a specific section.
        /// </summary>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>The last order number if found; otherwise, 0.</returns>
        [HttpGet("last-order")]
        public async Task<IActionResult> GetLastOrderNumberFromSection([FromQuery] int sectionId)
        {
            var lastOrder = await repository.GetLastOrderNumberFromSectionAsync(sectionId);
            return Ok(lastOrder);
        }

        /// <summary>
        /// Adds a list of new exercises to a quiz.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <param name="exercises">The exercises to add.</param>
        /// <returns>Ok if added.</returns>
        [HttpPost("add-exercises")]
        public async Task<IActionResult> AddExercisesToQuiz([FromForm] int quizId, [FromForm] List<int> exercises)
        {
            await repository.AddExercisesToQuizAsync(quizId, exercises);
            return Ok();
        }

        /// <summary>
        /// Adds a single exercise to a quiz.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <param name="exerciseId">The ID of the exercise to add.</param>
        /// <returns>Ok if added.</returns>
        [HttpPost("add-exercise")]
        public async Task<IActionResult> AddExerciseToQuiz([FromForm] int quizId, [FromForm] int exerciseId)
        {
            await repository.AddExerciseToQuizAsync(quizId, exerciseId);
            return Ok();
        }

        /// <summary>
        /// Removes an exercise from a quiz.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <param name="exerciseId">The ID of the exercise to remove.</param>
        /// <returns>Ok if removed.</returns>
        [HttpDelete("remove-exercise")]
        public async Task<IActionResult> RemoveExerciseFromQuiz([FromQuery] int quizId, [FromQuery] int exerciseId)
        {
            await repository.RemoveExerciseFromQuizAsync(quizId, exerciseId);
            return Ok();
        }

        /// <summary>
        /// Retrieves the quiz result for a specific quiz.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <returns>The quiz result.</returns>
        [HttpGet("get-result")]
        public async Task<IActionResult> GetQuizResult([FromQuery] int quizId)
        {
            var result = await repository.GetQuizResultAsync(quizId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
