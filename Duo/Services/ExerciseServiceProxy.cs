using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Exercises;

namespace Duo.Services
{
    public class ExerciseServiceProxy : IExerciseService
    {
        private readonly HttpClient httpClient;
        private string url = "https://localhost:7174/";

        public ExerciseServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateExercise(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new ArgumentNullException(nameof(exercise));
            }

            try
            {
                var response = await httpClient.PostAsJsonAsync($"{url}api/Exercise", exercise);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error creating exercise: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating exercise: {ex.Message}");
            }
        }

        public async Task DeleteExercise(int exerciseId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{url}api/Exercise/{exerciseId}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error deleting exercise: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            try
            {
                var response = await httpClient.GetAsync($"{url}api/Exercise");
                response.EnsureSuccessStatusCode();
                var exercises = await response.Content.ReadFromJsonAsync<List<Exercise>>();
                return exercises ?? new List<Exercise>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error fetching exercises: {ex.Message}");
                return new List<Exercise>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Exercise>();
            }
        }

        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{url}api/Exercise/exam/{examId}");
                response.EnsureSuccessStatusCode();
                var exercises = await response.Content.ReadFromJsonAsync<List<Exercise>>();
                return exercises ?? new List<Exercise>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error fetching exercises from exam: {ex.Message}");
                return new List<Exercise>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Exercise>();
            }
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{url}api/Exercise/quiz/{quizId}");
                response.EnsureSuccessStatusCode();
                var exercises = await response.Content.ReadFromJsonAsync<List<Exercise>>();
                return exercises ?? new List<Exercise>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error fetching exercises from quiz: {ex.Message}");
                return new List<Exercise>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Exercise>();
            }
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{url}api/Exercise/{exerciseId}");
                response.EnsureSuccessStatusCode();
                var exercise = await response.Content.ReadFromJsonAsync<Exercise>();
                return exercise ?? throw new InvalidOperationException("Exercise not found.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error fetching exercise by ID: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}
