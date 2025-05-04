using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Models.Quizzes;

namespace Duo.Services
{
    public class UserServiceProxy
    {
        private readonly HttpClient httpClient;
        private const string BaseUrl = "api/user";

        public UserServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
                }
                return await httpClient.GetFromJsonAsync<User>($"{BaseUrl}/{userId}");
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
                var users = await httpClient.GetFromJsonAsync<List<User>>(BaseUrl);
                return users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
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
                var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/register", user);
                response.EnsureSuccessStatusCode();

                var createdUser = await response.Content.ReadFromJsonAsync<User>();
                return createdUser.UserId;
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
                var user = await GetByIdAsync(userId);
                user.NumberOfCompletedSections = newNrOfSectionsCompleted;
                user.NumberOfCompletedQuizzesInSection = newNrOfQuizzesInSectionCompleted;

                await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
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

                await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
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
                await httpClient.PutAsJsonAsync($"{BaseUrl}", user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUserAsync: {ex.Message}");
            }
        }
    }
}