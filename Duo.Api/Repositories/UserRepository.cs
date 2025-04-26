
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Api.Models;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Duo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext m_context;

        public UserRepository(DataContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            m_context = context;
        }


        /// <summary>
        /// Retrieves a user by their username. Throws an exception if the user is not found or if the username is null or empty.
        /// </summary>
        /// <param name="username"> string describing a user's name </param>
        /// <returns> Returns user object </returns>
        /// <exception cref="ArgumentException"> thrown if username is null or emtpy </exception>
        /// <exception cref="KeyNotFoundException"> thown if user is not found </exception>
        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }

            var user = await m_context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with username '{username}' not found.");
            }

            return user;
        }


        /// <summary>
        /// Creates a new user in the database. Throws an exception if the username is null or empty, or if the user already exists.
        /// </summary>
        /// <param name="user"> user onject to add </param>
        /// <returns> Returns the id of the new user </returns>
        /// <exception cref="ArgumentNullException"> thrown if user is null </exception>"
        /// <exception cref="ArgumentException"> thrown if username is null or empty </exception>
        /// <exception cref="InvalidOperationException"> thrown if username is not unique </exception>
        public async Task<int> CreateUserAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(user));
            }

            var exists = await m_context.Users
                .AnyAsync(u => u.Username == user.Username);

            if (exists)
            {
                throw new InvalidOperationException($"Username '{user.Username}' already exists.");
            }

            m_context.Users.Add(user);
            await m_context.SaveChangesAsync();

            return user.UserId;
        }


        /// <summary>
        /// Updates the user's progress in the database. Throws an exception if the user ID is less than or equal to 0, or if the user is not found.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newNrOfSectionsCompleted"></param>
        /// <param name="newNrOfQuizzesCompletedInSection"></param>
        /// <exception cref="ArgumentException"> thrown if user id is negative or 0 </exception>
        /// <exception cref="KeyNotFoundException"> thrown if user was not found </exception>
        public async Task UpdateUserProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesCompletedInSection)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await m_context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            user.NumberOfCompletedSections = newNrOfSectionsCompleted;
            user.NumberOfCompletedQuizzesInSection = newNrOfQuizzesCompletedInSection;

            m_context.Users.Update(user);
            await m_context.SaveChangesAsync();
        }


        /// <summary>
        /// Retrieves a user by their ID. Throws an exception if the user ID is less than or equal to 0, or if the user is not found.
        /// </summary>
        /// <param name="userId"> id of the user </param>
        /// <returns> user object </returns>
        /// <exception cref="ArgumentException"> thrown if id is negative or 0 </exception>
        /// <exception cref="KeyNotFoundException"> thrown if user not found </exception>
        public async Task<User> GetByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await m_context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            return user;
        }

        /// <summary>
        /// Updates the user's progress in the database by incrementing the number of completed sections. Throws an exception if the user ID is less than or equal to 0, or if the user is not found.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException"> thrown if id is 0 or smaller </exception>
        /// <exception cref="KeyNotFoundException"> thrown if user was not found </exception>
        public async Task IncrementUserProgressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.");
            }

            var user = await m_context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            user.NumberOfCompletedSections += 1;

            m_context.Users.Update(user);
            await m_context.SaveChangesAsync();
        }
    }
}
