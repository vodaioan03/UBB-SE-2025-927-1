using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing users in the system.
    /// Provides endpoints for user registration, login, and CRUD operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [Route("user")]
    [ExcludeFromCodeCoverage]
    public class UserController(IRepository repository) : BaseController(repository)
    {
        #region Constructors

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user data to register.</param>
        /// <returns>The created user with a 201 status code.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                return BadRequest("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Email is required.");
            }

            await repository.AddUserAsync(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, new
            {
                user.UserId,
                user.Username,
                user.Email
            });
        }

        /// <summary>
        /// Logs in a user by verifying their email.
        /// </summary>
        /// <param name="loginRequest">The login request containing the user's email.</param>
        /// <returns>The user data if login is successful; otherwise, an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Login data is required.");
            }

            if (string.IsNullOrWhiteSpace(loginRequest.Email))
            {
                return BadRequest("Email cannot be empty.");
            }

            // Search for user by email
            var users = await repository.GetUsersFromDbAsync();
            var user = users.Find(u => u.Email == loginRequest.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email.");
            }

            // Log in time
            user.LastLoginTime = DateTime.UtcNow;
            await this.repository.UpdateUserAsync(user);

            return Ok(user);
    }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user data if found; otherwise, a 404 error.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await repository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">The updated user data.</param>
        /// <returns>No content if the update is successful; otherwise, an error message.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null || user.UserId <= 0)
            {
                return BadRequest("Valid user data is required.");
            }

            var existingUser = await repository.GetUserByIdAsync(user.UserId);

            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Update user properties
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.NumberOfCompletedSections = user.NumberOfCompletedSections;
            existingUser.NumberOfCompletedQuizzesInSection = user.NumberOfCompletedQuizzesInSection;

            await repository.UpdateUserAsync(existingUser);

            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if the deletion is successful; otherwise, a 404 error.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await repository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            await repository.DeleteUserAsync(id);

            return NoContent();
        }

        #endregion
    }
}
