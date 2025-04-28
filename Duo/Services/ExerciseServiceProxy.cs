using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Exercises;

namespace Duo.Services
{
    public class ExerciseServiceProxy : IExerciseService
    {
        private readonly HttpClient httpClient;

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

            var response = await httpClient.PostAsJsonAsync("api/Exercise", exercise);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteExercise(int exerciseId)
        {
            var response = await httpClient.DeleteAsync($"api/Exercise/{exerciseId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            var response = await httpClient.GetAsync("api/Exercise");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Exercise>>();
        }

        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            var response = await httpClient.GetAsync($"api/Exercise/exam/{examId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Exercise>>();
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            var response = await httpClient.GetAsync($"api/Exercise/quiz/{quizId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Exercise>>();
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            var response = await httpClient.GetAsync($"api/Exercise/{exerciseId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Exercise>();
        }
    }
}
