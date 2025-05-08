using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using Duo.Exceptions;
using Duo.Models.Quizzes;
using Duo.Models.Quizzes.API;
using Duo.Services.Interfaces;
using Duo.Helpers;
using Azure;
using Duo.Models.Exercises;

namespace Duo.Services
{
    public class QuizServiceProxy : IQuizServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174/api/";

        public QuizServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Quiz>> GetAsync()
        {
                var result = await httpClient.GetFromJsonAsync<List<Quiz>>($"{url}quiz/list");
                if (result == null)
                {
                    throw new QuizServiceProxyException("Received null response when fetching quiz list.");
                }
                return result;
        }

        public async Task<List<Exam>> GetAllAvailableExamsAsync()
        {
            var result = await httpClient.GetAsync($"{url}exam/get-available");
            if (result == null)
            {
                throw new QuizServiceProxyException("Received null response when fetching available exams.");
            }
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            var exams = new List<Exam>();
            using JsonDocument doc = JsonDocument.Parse(responseJson);

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var examJsonString = element.GetRawText();
                var exam = JsonSerializationUtil.DeserializeExamWithTypedExercises(examJsonString);
                exams.Add(exam);
            }

            return exams;
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
                var result = await httpClient.GetFromJsonAsync<Quiz>($"{url}quiz/get?id={id}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for quiz with ID {id}.");
                }
                return result;
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            var result = await httpClient.GetAsync($"{url}exam/get?id={id}");
            if (result == null)
            {
                throw new QuizServiceProxyException($"Received null response for exam with ID {id}.");
            }

            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            var exam = JsonSerializationUtil.DeserializeExamWithTypedExercises(responseJson);
            return exam;
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId)
        {
                var result = await httpClient.GetFromJsonAsync<List<Quiz>>($"{url}quiz/get-all-section?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for section {sectionId} quizzes.");
                }
                return result;
        }

        public async Task<int> CountQuizzesFromSectionAsync(int sectionId)
        {
                var result = await httpClient.GetFromJsonAsync<int?>($"{url}quiz/count-from-section?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response when counting quizzes in section {sectionId}.");
                }
                return result.Value;
        }

        public async Task<int> LastOrderNumberFromSectionAsync(int sectionId)
        {
                var result = await httpClient.GetFromJsonAsync<int?>($"{url}quiz/last-order?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response when getting last order number from section {sectionId}.");
                }
                return result.Value;
        }

        public async Task<Exam> GetExamFromSectionAsync(int sectionId)
        {
                var result = await httpClient.GetFromJsonAsync<Exam>($"{url}exam/get-from-section?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for exam from section {sectionId}.");
                }
                return result;
        }

        public async Task DeleteQuizAsync(int quizId)
        {
                await httpClient.DeleteAsync($"{url}quiz/delete?id={quizId}");
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
                await httpClient.PutAsJsonAsync($"{url}quiz/update", quiz);
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            // serialize json and print
            var json = System.Text.Json.JsonSerializer.Serialize(quiz);

            await httpClient.PostAsJsonAsync($"{url}quiz/add", quiz);
        }

        public async Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds)
        {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(quizId.ToString()), "quizId");
                foreach (var exerciseId in exerciseIds)
                {
                    formData.Add(new StringContent(exerciseId.ToString()), "exercises");
                }
                await httpClient.PostAsync($"{url}quiz/add-exercises", formData);
        }

        public async Task AddExerciseToQuizAsync(int quizId, int exerciseId)
        {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(quizId.ToString()), "quizId");
                formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");
                await httpClient.PostAsync($"{url}quiz/add-exercise", formData);
        }

        public async Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId)
        {
                await httpClient.DeleteAsync($"{url}quiz/remove-exercise?quizId={quizId}&exerciseId={exerciseId}");
        }

        public async Task DeleteExamAsync(int examId)
        {
                await httpClient.DeleteAsync($"{url}exam/delete?id={examId}");
        }

        public async Task UpdateExamAsync(Exam exam)
        {
                await httpClient.PutAsJsonAsync($"{url}exam/update", exam);
        }

        public async Task CreateExamAsync(Exam exam)
        {
            try
            {
                string serialized = JsonSerializationUtil.SerializeExamWithTypedExercises(exam);
                await httpClient.PostAsync($"{url}exam/add", new StringContent(serialized, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task<QuizResult> GetResultAsync(int quizId)
        {
                var result = await httpClient.GetFromJsonAsync<QuizResult>($"{url}quiz/get-result?quizId={quizId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for result of quiz {quizId}.");
                }
                return result;
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
                await httpClient.PostAsJsonAsync($"{url}quiz/submit", submission);
        }
    }
}
