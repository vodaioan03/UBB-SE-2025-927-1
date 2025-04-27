using Microsoft.AspNetCore.Mvc;
using Duo.Api.Models;
using Duo.Api.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("course")]
    public class CourseController : BaseController
    {
        public CourseController(DataContext dataContext) : base(dataContext) { }

        /// <summary>
        /// Adds a new course to the database.
        /// </summary>
        /// <param name="course">The course data to add.</param>
        /// <returns>The added course.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddCourse([FromForm] Course course)
        {
            dataContext.Courses.Add(course);
            await dataContext.SaveChangesAsync();
            return Ok(course);
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>The course if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetCourse([FromQuery] int id)
        {
            var course = await dataContext.Courses.FindAsync(id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }

        /// <summary>
        /// Lists all courses in the database.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListCourses()
        {
            var courses = await dataContext.Courses.ToListAsync();
            return Ok(courses);
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="updatedCourse">The updated course data, including CourseId.</param>
        /// <returns>The updated course if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse([FromForm] Course updatedCourse)
        {
            var course = await dataContext.Courses.FindAsync(updatedCourse.CourseId);
            if (course == null)
                return NotFound();
            dataContext.Entry(course).CurrentValues.SetValues(updatedCourse);
            await dataContext.SaveChangesAsync();
            return Ok(course);
        }

        /// <summary>
        /// Deletes a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCourse([FromQuery] int id)
        {
            var course = await dataContext.Courses.FindAsync(id);
            if (course == null)
                return NotFound();
            dataContext.Courses.Remove(course);
            await dataContext.SaveChangesAsync();
            return Ok();
        }
    }
}