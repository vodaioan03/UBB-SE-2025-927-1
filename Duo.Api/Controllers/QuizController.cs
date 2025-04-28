using Microsoft.AspNetCore.Mvc;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using System.Threading.Tasks;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("quiz")]
    public class QuizController : BaseController
    {
        private readonly IRepository repository;

        public QuizController(DataContext dataContext, IRepository repository) : base(dataContext)
        {
            this.repository = repository;
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
        /// Retrieves a quiz by its ID .
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
    }
}
