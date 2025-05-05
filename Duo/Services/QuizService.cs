using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Exceptions;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Quizzes.API;

namespace Duo.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuizServiceProxy serviceProxy;

        public QuizService(QuizServiceProxy serviceProxy)
        {
            this.serviceProxy = serviceProxy;
        }

        public async Task<List<Quiz>> Get()
        {
            try
            {
                return await serviceProxy.GetAsync();
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
                return await serviceProxy.GetAllAvailableExamsAsync();
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
                return await serviceProxy.GetQuizByIdAsync(quizId);
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
                return await serviceProxy.GetExamByIdAsync(examId);
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
                return await serviceProxy.GetAllQuizzesFromSectionAsync(sectionId);
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
                return await serviceProxy.CountQuizzesFromSectionAsync(sectionId);
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
                return await serviceProxy.LastOrderNumberFromSectionAsync(sectionId);
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
                return await serviceProxy.GetExamFromSectionAsync(sectionId);
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
                await serviceProxy.DeleteQuizAsync(quizId);
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
                await serviceProxy.UpdateQuizAsync(quiz);
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
                await serviceProxy.CreateQuizAsync(quiz);
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
                var exerciseIds = new List<int>();
                foreach (var exercise in exercises)
                {
                    exerciseIds.Add(exercise.Id);
                }
                await serviceProxy.AddExercisesToQuizAsync(quizId, exerciseIds);
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
                await serviceProxy.AddExerciseToQuizAsync(quizId, exerciseId);
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
                await serviceProxy.RemoveExerciseFromQuizAsync(quizId, exerciseId);
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
                await serviceProxy.DeleteExamAsync(examId);
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
                await serviceProxy.UpdateExamAsync(exam);
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
                await serviceProxy.CreateExamAsync(exam);
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
                await serviceProxy.SubmitQuizAsync(submission);
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
                return await serviceProxy.GetResultAsync(quizId);
            }
            catch (Exception ex)
            {
                throw new QuizServiceException($"Failed to get result for quiz with ID {quizId}.", ex);
            }
        }
    }
}