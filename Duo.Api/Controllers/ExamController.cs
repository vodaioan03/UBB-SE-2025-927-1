using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models.Quizzes;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing exams, including CRUD operations and additional functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExamController"/> class with the specified repository.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExamController"/> class.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class ExamController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Adds a new exam to the database.
        /// </summary>
        /// <param name="exam">The exam data to add.</param>
        /// <returns>The added exam.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddExam([FromForm] Exam exam)
        {
            try
            {
                await repository.AddExamAsync(exam);
                return Ok(exam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to retrieve.</param>
        /// <returns>The exam if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExam([FromQuery] int id)
        {
            try
            {
                var exam = await repository.GetExamByIdAsync(id);
                if (exam == null)
                {
                    return NotFound();
                }
                return Ok(exam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lists all exams in the database.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListExams()
        {
            try
            {
                var exams = await repository.GetExamsFromDbAsync();
                return Ok(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="updatedExam">The updated exam data, including Id.</param>
        /// <returns>The updated exam if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateExam([FromForm] Exam updatedExam)
        {
            try
            {
                var exam = await repository.GetExamByIdAsync(updatedExam.Id);
                if (exam == null)
                {
                    return NotFound();
                }

                await repository.UpdateExamAsync(updatedExam);
                return Ok(updatedExam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteExam([FromQuery] int id)
        {
            try
            {
                var exam = await repository.GetExamByIdAsync(id);
                if (exam == null)
                {
                    return NotFound();
                }

                await repository.DeleteExamAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the exam associated with a specific section.
        /// </summary>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>The exam if found; otherwise, NotFound.</returns>
        [HttpGet("get-from-section")]
        public async Task<IActionResult> GetExamFromSection([FromQuery] int sectionId)
        {
            try
            {
                var exam = await repository.GetExamFromSectionAsync(sectionId);
                if (exam == null)
                {
                    return NotFound();
                }

                return Ok(exam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all available exams.
        /// </summary>
        /// <returns>A list of available exams.</returns>
        [HttpGet("get-available")]
        public async Task<IActionResult> GetAvailableExams()
        {
            try
            {
                var exams = await repository.GetAvailableExamsAsync();
                return Ok(exams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion
    }
}