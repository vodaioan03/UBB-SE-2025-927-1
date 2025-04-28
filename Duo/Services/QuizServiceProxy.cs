using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Duo.Models.Quizzes;

namespace Duo.Services
{
    public class QuizServiceProxy
    {
        private readonly HttpClient httpClient;

        public QuizServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Quiz>> GetAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Quiz>>("quiz/list");
        }

        public async Task<List<Quiz>> GetAllAvailableExamsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Quiz>>("exam/get-available");
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Quiz>($"quiz/get?id={id}");
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Exam>($"exam/get?id={id}");
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId)
        {
            return await httpClient.GetFromJsonAsync<List<Quiz>>($"quiz/get-all-section?sectionId={sectionId}");
        }

        public async Task<int> CountQuizzesFromSectionAsync(int sectionId)
        {
            return await httpClient.GetFromJsonAsync<int>($"quiz/count-from-section?sectionId={sectionId}");
        }

        public async Task<int> LastOrderNumberFromSectionAsync(int sectionId)
        {
            return await httpClient.GetFromJsonAsync<int>($"quiz/last-order?sectionId={sectionId}");
        }

        public async Task<Exam> GetExamFromSectionAsync(int sectionId)
        {
            return await httpClient.GetFromJsonAsync<Exam>($"exam/get-from-section?sectionId={sectionId}");
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            await httpClient.DeleteAsync($"quiz/delete?id={quizId}");
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            await httpClient.PutAsJsonAsync("quiz/update", quiz);
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            await httpClient.PostAsJsonAsync("quiz/add", quiz);
        }

        public async Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(quizId.ToString()), "quizId");
            foreach (var exerciseId in exerciseIds)
            {
                formData.Add(new StringContent(exerciseId.ToString()), "exercises");
            }

            await httpClient.PostAsync("quiz/add-exercises", formData);
        }

        public async Task AddExerciseToQuizAsync(int quizId, int exerciseId)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(quizId.ToString()), "quizId");
            formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");

            await httpClient.PostAsync("quiz/add-exercise", formData);
        }

        public async Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId)
        {
            await httpClient.DeleteAsync($"quiz/remove-exercise?quizId={quizId}&exerciseId={exerciseId}");
        }

        public async Task DeleteExamAsync(int examId)
        {
            await httpClient.DeleteAsync($"exam/delete?id={examId}");
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            await httpClient.PutAsJsonAsync("exam/update", exam);
        }

        public async Task CreateExamAsync(Exam exam)
        {
            await httpClient.PostAsJsonAsync("exam/add", exam);
        }

        public async Task<object> GetResultAsync(int quizId)
        {
            return await httpClient.GetFromJsonAsync<object>($"quiz/get-result?quizId={quizId}");
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
            await httpClient.PostAsJsonAsync("quiz/submit", submission);
        }
    }
}
