using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Duo.Models.Quizzes;
using Duo.Models.Quizzes.API;

namespace Duo.Services
{
    public class QuizServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174/";

        public QuizServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Quiz>> GetAsync()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<List<Quiz>>($"{url}quiz/list");
                if (result == null)
                {
                    throw new QuizServiceProxyException("Received null response when fetching quiz list.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException("Failed to fetch quiz list.", ex);
            }
        }

        public async Task<List<Exam>> GetAllAvailableExamsAsync()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<List<Exam>>($"{url}exam/get-available");
                if (result == null)
                {
                    throw new QuizServiceProxyException("Received null response when fetching available exams.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException("Failed to fetch available exams.", ex);
            }
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<Quiz>($"{url}quiz/get?id={id}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for quiz with ID {id}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to fetch quiz with ID {id}.", ex);
            }
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<Exam>($"{url}exam/get?id={id}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for exam with ID {id}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to fetch exam with ID {id}.", ex);
            }
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<List<Quiz>>($"{url}quiz/get-all-section?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for section {sectionId} quizzes.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to fetch quizzes from section {sectionId}.", ex);
            }
        }

        public async Task<int> CountQuizzesFromSectionAsync(int sectionId)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<int?>($"{url}quiz/count-from-section?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response when counting quizzes in section {sectionId}.");
                }
                return result.Value;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to count quizzes from section {sectionId}.", ex);
            }
        }

        public async Task<int> LastOrderNumberFromSectionAsync(int sectionId)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<int?>($"{url}quiz/last-order?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response when getting last order number from section {sectionId}.");
                }
                return result.Value;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to get last order number from section {sectionId}.", ex);
            }
        }

        public async Task<Exam> GetExamFromSectionAsync(int sectionId)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<Exam>($"{url}exam/get-from-section?sectionId={sectionId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for exam from section {sectionId}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to fetch exam from section {sectionId}.", ex);
            }
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            try
            {
                await httpClient.DeleteAsync($"{url}quiz/delete?id={quizId}");
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to delete quiz with ID {quizId}.", ex);
            }
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            try
            {
                await httpClient.PutAsJsonAsync($"{url}quiz/update", quiz);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to update quiz with ID {quiz.Id}.", ex);
            }
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            try
            {
                await httpClient.PostAsJsonAsync($"{url}quiz/add", quiz);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException("Failed to create quiz.", ex);
            }
        }

        public async Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds)
        {
            try
             {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(quizId.ToString()), "quizId");
                foreach (var exerciseId in exerciseIds)
                {
                    formData.Add(new StringContent(exerciseId.ToString()), "exercises");
                }
                await httpClient.PostAsync($"{url}quiz/add-exercises", formData);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to add exercises to quiz {quizId}.", ex);
            }
        }

        public async Task AddExerciseToQuizAsync(int quizId, int exerciseId)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(quizId.ToString()), "quizId");
                formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");
                await httpClient.PostAsync($"{url}quiz/add-exercise", formData);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to add exercise {exerciseId} to quiz {quizId}.", ex);
            }
        }

        public async Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId)
        {
            try
            {
                await httpClient.DeleteAsync($"{url}quiz/remove-exercise?quizId={quizId}&exerciseId={exerciseId}");
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to remove exercise {exerciseId} from quiz {quizId}.", ex);
            }
        }

        public async Task DeleteExamAsync(int examId)
        {
            try
            {
                await httpClient.DeleteAsync($"{url}exam/delete?id={examId}");
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to delete exam with ID {examId}.", ex);
            }
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            try
            {
                await httpClient.PutAsJsonAsync($"{url}exam/update", exam);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to update exam with ID {exam.Id}.", ex);
            }
        }

        public async Task CreateExamAsync(Exam exam)
        {
            try
            {
                await httpClient.PostAsJsonAsync($"{url}exam/add", exam);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException("Failed to create exam.", ex);
            }
        }

        public async Task<QuizResult> GetResultAsync(int quizId)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<QuizResult>($"{url}quiz/get-result?quizId={quizId}");
                if (result == null)
                {
                    throw new QuizServiceProxyException($"Received null response for result of quiz {quizId}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException($"Failed to fetch result for quiz with ID {quizId}.", ex);
            }
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
            try
            {
                await httpClient.PostAsJsonAsync($"{url}quiz/submit", submission);
            }
            catch (Exception ex)
            {
                throw new QuizServiceProxyException("Failed to submit quiz.", ex);
            }
        }
    }

    [Serializable]
    public class QuizServiceProxyException : Exception
    {
        public QuizServiceProxyException()
        {
        }

        public QuizServiceProxyException(string? message) : base(message)
        {
        }

        public QuizServiceProxyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
