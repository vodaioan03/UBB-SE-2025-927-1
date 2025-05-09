using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
// using ABI.System;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;

namespace Duo.Services
{
    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly HttpClient httpClient;
        private const string BaseUrl = "https://localhost:7174/api/User";

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
            var response = await httpClient.GetAsync($"{BaseUrl}/{userId}");
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(responseJson);

            User newUser = new User()
            {
                UserId = doc.RootElement.GetProperty("userId").GetInt32(),
                Username = doc.RootElement.GetProperty("username").GetString(),
                NumberOfCompletedSections = doc.RootElement.GetProperty("numberOfCompletedSections").GetInt32(),
                NumberOfCompletedQuizzesInSection = doc.RootElement.GetProperty("numberOfCompletedQuizzesInSection").GetInt32()
            };

            return newUser;
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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
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

            await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
        }

        public async Task IncrementUserProgressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }
            var user = await GetByIdAsync(userId);
            user.NumberOfCompletedQuizzesInSection++;

            await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await httpClient.PutAsJsonAsync($"{BaseUrl}", user);
        }
    }
}