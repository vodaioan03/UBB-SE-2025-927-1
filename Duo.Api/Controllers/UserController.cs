using Duo.Api.Controllers;
using Duo.Api.Models;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the UserController class.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="dataContext"></param>
    public UserController(IRepository repository): base(repository)
    {
    }

    // POST: /register
    /// <summary>
    /// Registers a new user.
    /// </summary>
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

        await this.repository.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, new
        {
            user.UserId,
            user.Username,
            user.Email
        });
    }

    // POST: /login
    /// <summary>
    /// Logs in a user.
    /// </summary>
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
        var users = await this.repository.GetUsersFromDbAsync();
        var user = users.Find(u => u.Email == loginRequest.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email.");
        }

        return Ok(user);
    }

    // GET: /{id}
    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await this.repository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }

    // PUT: /update
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        if (user == null || user.UserId <= 0)
        {
            return BadRequest("Valid User data is required.");
        }

        var existingUser = await this.repository.GetUserByIdAsync(user.UserId);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.NumberOfCompletedSections = user.NumberOfCompletedSections;
        existingUser.NumberOfCompletedQuizzesInSection = user.NumberOfCompletedQuizzesInSection;

        await this.repository.UpdateUserAsync(existingUser);
        return NoContent();
    }

    // DELETE: /delete/{id}
    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await this.repository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        await this.repository.DeleteUserAsync(id);
        return NoContent();
    }
}
