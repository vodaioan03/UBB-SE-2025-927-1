﻿using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing courses in the system.
    /// Provides endpoints for adding, retrieving, updating, deleting, and managing user interactions with courses.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CourseController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance to be used for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CourseController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Adds a new course to the database.
        /// </summary>
        /// <param name="course">The course data to add.</param>
        /// <returns>The added course.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddCourse([FromForm] Course course)
        {
            await repository.AddCourseAsync(course);
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
            var course = await repository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        /// <summary>
        /// Lists all courses in the database.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListCourses()
        {
            var courses = await repository.GetCoursesFromDbAsync();
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
            var course = await repository.GetCourseByIdAsync(updatedCourse.CourseId);
            if (course == null)
            {
                return NotFound();
            }
            await repository.UpdateCourseAsync(updatedCourse);
            return Ok(updatedCourse);
        }

        /// <summary>
        /// Deletes a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCourse([FromQuery] int id)
        {
            var course = await repository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            await repository.DeleteCourseAsync(id);
            return Ok();
        }

        /// <summary>
        /// Enrolls a user in a course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>Ok if enrollment is successful.</returns>
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromForm] int userId, [FromForm] int courseId)
        {
            await repository.EnrollUserInCourseAsync(userId, courseId);
            return Ok();
        }

        /// <summary>
        /// Checks if a user is enrolled in a course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if enrolled; otherwise, false.</returns>
        [HttpGet("is-enrolled")]
        public async Task<IActionResult> IsEnrolled([FromQuery] int userId, [FromQuery] int courseId)
        {
            var isEnrolled = await repository.IsUserEnrolledInCourseAsync(userId, courseId);
            return Ok(isEnrolled);
        }

        /// <summary>
        /// Checks if a course is completed by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if completed; otherwise, false.</returns>
        [HttpGet("is-completed")]
        public async Task<IActionResult> IsCompleted([FromForm] int userId, [FromForm] int courseId)
        {
            var isCompleted = await repository.IsCourseCompletedAsync(userId, courseId);
            return Ok(isCompleted);
        }

        /// <summary>
        /// Filters courses based on search text, type, enrollment status, and tags.
        /// </summary>
        /// <param name="searchText">The text to search for in course titles.</param>
        /// <param name="filterPremium">Whether to filter premium courses.</param>
        /// <param name="filterFree">Whether to filter free courses.</param>
        /// <param name="filterEnrolled">Whether to filter enrolled courses.</param>
        /// <param name="filterNotEnrolled">Whether to filter not enrolled courses.</param>
        /// <returns>A filtered list of courses.</returns>
        [HttpGet("get-filtered")]
        public async Task<IActionResult> GetFilteredCourses(
            [FromQuery] string searchText,
            [FromQuery] bool filterPremium,
            [FromQuery] bool filterFree,
            [FromQuery] bool filterEnrolled,
            [FromQuery] bool filterNotEnrolled)
        {
            var userId = 1; // Hardcoded for simplicity, replace this with actual UserId based on your auth system
            var courses = await repository.GetFilteredCoursesAsync(searchText, filterPremium, filterFree, filterEnrolled, filterNotEnrolled, userId);
            return Ok(courses);
        }

        /// <summary>
        /// Updates the time a user spent on a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <param name="timeInSeconds">The time in seconds to add.</param>
        /// <returns>Ok if updated.</returns>
        [HttpPut("update-time")]
        public async Task<IActionResult> UpdateTime([FromForm] int userId, [FromForm] int courseId, [FromForm] int timeInSeconds)
        {
            await repository.UpdateTimeSpentAsync(userId, courseId, timeInSeconds);
            return Ok();
        }

        /// <summary>
        /// Claims the completion reward for a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <returns>Ok if reward claimed.</returns>
        [HttpPost("claim-completion")]
        public async Task<IActionResult> ClaimCompletionReward([FromForm] int userId, [FromForm] int courseId)
        {
            await repository.ClaimCompletionRewardAsync(userId, courseId);
            return Ok();
        }

        /// <summary>
        /// Claims the time reward for a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <returns>Ok if reward claimed.</returns>
        [HttpPost("claim-time")]
        public async Task<IActionResult> ClaimTimeReward([FromForm] int userId, [FromForm] int courseId)
        {
            await repository.ClaimTimeRewardAsync(userId, courseId);
            return Ok();
        }

        /// <summary>
        /// Gets the time a user has spent on a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <returns>The time spent in seconds.</returns>
        [HttpGet("get-time")]
        public async Task<IActionResult> GetTimeSpent([FromQuery] int userId, [FromQuery] int courseId)
        {
            var timeSpent = await repository.GetTimeSpentAsync(userId, courseId);
            return Ok(timeSpent);
        }

        /// <summary>
        /// Gets the time limit of a course.
        /// </summary>
        /// <param name="courseId">The course ID.</param>
        /// <returns>The time limit in seconds.</returns>
        [HttpGet("get-time-limit")]
        public async Task<IActionResult> GetTimeLimit([FromQuery] int courseId)
        {
            var course = await repository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course.TimeToComplete);
        }

        [HttpGet("completed-modules-count")]
        public async Task<IActionResult> GetCompletedModulesCount([FromQuery] int userId, [FromQuery] int courseId)
        {
            var completedModules = await repository.GetCompletedModulesCountAsync(userId, courseId);
            return Ok(completedModules);
        }

        [HttpGet("required-modules-count")]
        public async Task<IActionResult> GetRequiredModulesCount([FromQuery] int userId, [FromQuery] int courseId)
        {
            var completedModules = await repository.GetCompletedModulesCountAsync(userId, courseId);
            return Ok(completedModules);
        }
        #endregion
    }
}