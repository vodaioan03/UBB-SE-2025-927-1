using System.ComponentModel.DataAnnotations;

namespace Duo.Api.Models;

public class User
{
    /// <summary>
    /// Represents a user of the platform.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of completed sections by the user.
        /// </summary>
        public int NumberOfCompletedSections { get; set; }

        /// <summary>
        /// Gets or sets the number of completed quizzes in the current section.
        /// </summary>
        public int NumberOfCompletedQuizzesInSection { get; set; }

        /// <summary>
        /// Gets or sets the email of the user (optional).
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }
    }
}
