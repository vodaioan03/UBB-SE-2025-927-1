using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Models.Quizzes;

namespace Duo.Services
{
    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly HttpClient httpClient;
        private const string BaseUrl = "api/user";

        public UserServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }
            return await httpClient.GetFromJsonAsync<User>($"{BaseUrl}/{userId}");
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }
            var users = await httpClient.GetFromJsonAsync<List<User>>(BaseUrl);
            return users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> CreateUserAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/register", user);
            response.EnsureSuccessStatusCode();

            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            return createdUser.UserId;
        }

        public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await GetByIdAsync(userId);
            user.NumberOfCompletedSections = newNrOfSectionsCompleted;
            user.NumberOfCompletedQuizzesInSection = newNrOfQuizzesInSectionCompleted;

            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task IncrementUserProgressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await GetByIdAsync(userId);
            user.NumberOfCompletedQuizzesInSection++;

            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}", user);
            response.EnsureSuccessStatusCode();
        }
    }
}
