using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Exceptions;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Quizzes.API;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizServiceProxy serviceProxy;

        /// <summary>
        /// For production: accepts the concrete proxy, up-casts to the interface.
        /// </summary>
        public QuizService(QuizServiceProxy concreteProxy)
            : this((IQuizServiceProxy)concreteProxy)
        {
        }

        /// <summary>
        /// Testable constructor: accepts the proxy interface directly.
        /// </summary>
        public QuizService(IQuizServiceProxy serviceProxy)
        {
            this.serviceProxy = serviceProxy ?? throw new ArgumentNullException(nameof(serviceProxy));
        }

        public async Task<List<Quiz>> Get()
        {
            try
            {
                return await serviceProxy.GetAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException("Failed to get quizzes.", ex);
            }
        }

        public async Task<List<Exam>> GetAllAvailableExams()
        {
            try
            {
                return await serviceProxy.GetAllAvailableExamsAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException("Failed to retrieve available exams.", ex);
            }
        }

        public async Task<Quiz> GetQuizById(int quizId)
        {
            try
            {
                return await serviceProxy.GetQuizByIdAsync(quizId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get quiz with ID {quizId}.", ex);
            }
        }

        public async Task<Exam> GetExamById(int examId)
        {
            try
            {
                return await serviceProxy.GetExamByIdAsync(examId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get exam with ID {examId}.", ex);
            }
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSection(int sectionId)
        {
            try
            {
                return await serviceProxy.GetAllQuizzesFromSectionAsync(sectionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get quizzes from section {sectionId}.", ex);
            }
        }

        public async Task<int> CountQuizzesFromSection(int sectionId)
        {
            try
            {
                return await serviceProxy.CountQuizzesFromSectionAsync(sectionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to count quizzes in section {sectionId}.", ex);
            }
        }

        public async Task<int> LastOrderNumberFromSection(int sectionId)
        {
            try
            {
                return await serviceProxy.LastOrderNumberFromSectionAsync(sectionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get last order number from section {sectionId}.", ex);
            }
        }

        public async Task<Exam?> GetExamFromSection(int sectionId)
        {
            try
            {
                return await serviceProxy.GetExamFromSectionAsync(sectionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get exam from section {sectionId}.", ex);
            }
        }

        public async Task DeleteQuiz(int quizId)
        {
            try
            {
                await serviceProxy.DeleteQuizAsync(quizId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to delete quiz with ID {quizId}.", ex);
            }
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            try
            {
                await serviceProxy.UpdateQuizAsync(quiz).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to update quiz with ID {quiz.Id}.", ex);
            }
        }

        public async Task<int> CreateQuiz(Quiz quiz)
        {
            try
            {
                await serviceProxy.CreateQuizAsync(quiz).ConfigureAwait(false);
                return quiz.Id;
            }
            catch (Exception ex)
            {
                throw new QuizServiceException("Failed to create quiz.", ex);
            }
        }

        public async Task AddExercisesToQuiz(int quizId, List<Exercise> exercises)
        {
            try
            {
                var ids = new List<int>();
                foreach (var ex in exercises)
                {
                    ids.Add(ex.Id);
                }

                await serviceProxy.AddExercisesToQuizAsync(quizId, ids).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to add exercises to quiz with ID {quizId}.", ex);
            }
        }

        public async Task AddExerciseToQuiz(int quizId, int exerciseId)
        {
            try
            {
                await serviceProxy.AddExerciseToQuizAsync(quizId, exerciseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to add exercise {exerciseId} to quiz {quizId}.", ex);
            }
        }

        public async Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
        {
            try
            {
                await serviceProxy.RemoveExerciseFromQuizAsync(quizId, exerciseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to remove exercise {exerciseId} from quiz {quizId}.", ex);
            }
        }

        public async Task DeleteExam(int examId)
        {
            try
            {
                await serviceProxy.DeleteExamAsync(examId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to delete exam with ID {examId}.", ex);
            }
        }

        public async Task UpdateExam(Exam exam)
        {
            try
            {
                await serviceProxy.UpdateExamAsync(exam).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to update exam with ID {exam.Id}.", ex);
            }
        }

        public async Task<int> CreateExam(Exam exam)
        {
            try
            {
                await serviceProxy.CreateExamAsync(exam).ConfigureAwait(false);
                return exam.Id;
            }
            catch (Exception ex)
            {
                throw new QuizServiceException("Failed to create exam.", ex);
            }
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
            try
            {
                await serviceProxy.SubmitQuizAsync(submission).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException("Failed to submit quiz.", ex);
            }
        }

        public async Task<QuizResult> GetResultAsync(int quizId)
        {
            try
            {
                return await serviceProxy.GetResultAsync(quizId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get result for quiz with ID {quizId}.", ex);
            }
        }
    }
}
