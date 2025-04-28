using Microsoft.AspNetCore.Mvc;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using System.Threading.Tasks;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("exam")]
    public class ExamController : BaseController
    {
        public ExamController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Adds a new exam to the database.
        /// </summary>
        /// <param name="exam">The exam data to add.</param>
        /// <returns>The added exam.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddExam([FromForm] Exam exam)
        {
            await repository.AddExamAsync(exam);
            return Ok(exam);
        }

        /// <summary>
        /// Retrieves an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to retrieve.</param>
        /// <returns>The exam if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExam([FromQuery] int id)
        {
            var exam = await repository.GetExamByIdAsync(id);
            if (exam == null)
                return NotFound();
            return Ok(exam);
        }

        /// <summary>
        /// Lists all exams in the database.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListExams()
        {
            var exams = await repository.GetExamsFromDbAsync();
            return Ok(exams);
        }

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="updatedExam">The updated exam data, including Id.</param>
        /// <returns>The updated exam if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateExam([FromForm] Exam updatedExam)
        {
            var exam = await repository.GetExamByIdAsync(updatedExam.Id);
            if (exam == null)
                return NotFound();
            await repository.UpdateExamAsync(updatedExam);
            return Ok(updatedExam);
        }

        /// <summary>
        /// Deletes an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteExam([FromQuery] int id)
        {
            var exam = await repository.GetExamByIdAsync(id);
            if (exam == null)
                return NotFound();
            await repository.DeleteExamAsync(id);
            return Ok();
        }

        /// <summary>
        /// Retrieves the exam associated with a specific section.
        /// </summary>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>The exam if found; otherwise, NotFound.</returns>
        [HttpGet("get-from-section")]
        public async Task<IActionResult> GetExamFromSection([FromQuery] int sectionId)
        {
            var exam = await repository.GetExamFromSectionAsync(sectionId);
            if (exam == null)
                return NotFound();
            return Ok(exam);
        }

        /// <summary>
        /// Retrieves all available exams.
        /// </summary>
        /// <returns>A list of available exams.</returns>
        [HttpGet("get-available")]
        public async Task<IActionResult> GetAvailableExams()
        {
            var exams = await repository.GetAvailableExamsAsync();
            return Ok(exams);
        }
    }
}
