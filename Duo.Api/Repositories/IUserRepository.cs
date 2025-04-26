using System.Threading.Tasks;
using Duo.Api.Models;

namespace Duo.Api.Repositories
{
    /// <summary>
    /// Interface for user repository to manage user-related data operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> GetByUsernameAsync(string username);

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> CreateUserAsync(User user);

        /// <summary>
        /// Updates the user's progress in the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newNrOfSectionsCompleted"></param>
        /// <param name="newNrOfQuizzesCompletedInSection"></param>
        /// <returns></returns>
        Task UpdateUserProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesCompletedInSection);

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetByIdAsync(int userId);

        /// <summary>
        /// Increments the user's progress in the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task IncrementUserProgressAsync(int userId);
    }
}

