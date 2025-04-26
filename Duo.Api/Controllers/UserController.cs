using Duo.Api.Models;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserRepository m_userRepository;

        public UserController(DataContext context, IUserRepository userRepository) : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(userRepository);

            m_userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>The user object.</returns>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">If the user was not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetById(int id)
        {
            try
            {
                var user = await m_userRepository.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>The user object.</returns>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">If the user was not found.</response>
        [HttpGet("by-username/{username}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            try
            {
                var user = await m_userRepository.GetByUsernameAsync(username);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The ID of the newly created user.</returns>
        /// <response code="201">Returns the new user's ID.</response>
        /// <response code="400">If the input is invalid or username already exists.</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateUser(User user)
        {
            try
            {
                var newUserId = await m_userRepository.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = newUserId }, newUserId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the user's progress (completed sections and quizzes).
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="completedSections">New number of completed sections.</param>
        /// <param name="completedQuizzes">New number of completed quizzes in section.</param>
        /// <response code="204">Progress updated successfully.</response>
        /// <response code="400">If the input is invalid.</response>
        /// <response code="404">If the user was not found.</response>
        [HttpPatch("{id:int}/progress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProgress(int id, [FromQuery] int completedSections, [FromQuery] int completedQuizzes)
        {
            try
            {
                await m_userRepository.UpdateUserProgressAsync(id, completedSections, completedQuizzes);
                return NoContent(); // 204 No Content indicates successful update without returning data
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request for invalid input
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found if the user is not found
            }
        }

        /// <summary>
        /// Increments the user's number of completed sections by 1.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <response code="204">Progress incremented successfully.</response>
        /// <response code="400">If the input is invalid.</response>
        /// <response code="404">If the user was not found.</response>
        [HttpPatch("{id:int}/increment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> IncrementProgress(int id)
        {
            try
            {
                await m_userRepository.IncrementUserProgressAsync(id);
                return NoContent(); // 204 No Content indicates successful increment without returning data
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request for invalid input
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found if the user is not found
            }
        }

    }
}
