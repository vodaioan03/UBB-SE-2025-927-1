using System;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models;

namespace Duo.Services
{
    public class UserService : IUserService
    {
        private readonly IUserServiceProxy userServiceProxy;

        public UserService(IUserServiceProxy userServiceProxy)
        {
            this.userServiceProxy = userServiceProxy ?? throw new ArgumentNullException(nameof(userServiceProxy));
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
                }
                return await userServiceProxy.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new ArgumentException("Username cannot be null or empty.", nameof(username));
                }
                return await userServiceProxy.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByUsernameAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<int> CreateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                if (string.IsNullOrWhiteSpace(user.Username))
                {
                    throw new ArgumentException("Username cannot be null or empty.", nameof(user));
                }
                return await userServiceProxy.CreateUserAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateUserAsync: {ex.Message}");
                return -1;
            }
        }

        public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
                }
                await userServiceProxy.UpdateUserSectionProgressAsync(
                    userId,
                    newNrOfSectionsCompleted,
                    newNrOfQuizzesInSectionCompleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUserSectionProgressAsync: {ex.Message}");
            }
        }

        public async Task IncrementUserProgressAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
                }
                var user = await GetByIdAsync(userId);
                user.NumberOfCompletedQuizzesInSection++;

                await userServiceProxy.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IncrementUserProgressAsync: {ex.Message}");
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                await userServiceProxy.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUserAsync: {ex.Message}");
            }
        }
    }
}