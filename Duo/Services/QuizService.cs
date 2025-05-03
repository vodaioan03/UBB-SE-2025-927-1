using System.Collections.Generic;
using System.Threading.Tasks;
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
            return await serviceProxy.GetAsync();
        }
        public async Task<List<Exam>> GetAllAvailableExams()
        {
            // return await serviceProxy.GetAllAvailableExamsAsync();
            throw new System.NotImplementedException();
        }
        public async Task<Quiz> GetQuizById(int quizId)
        {
            return await serviceProxy.GetQuizByIdAsync(quizId);
        }

        public async Task<Exam> GetExamById(int examId)
        {
            return await serviceProxy.GetExamByIdAsync(examId);
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSection(int sectionId)
        {
            return await serviceProxy.GetAllQuizzesFromSectionAsync(sectionId);
        }

        public async Task<int> CountQuizzesFromSection(int sectionId)
        {
            return await serviceProxy.CountQuizzesFromSectionAsync(sectionId);
        }

        public async Task<int> LastOrderNumberFromSection(int sectionId)
        {
            return await serviceProxy.LastOrderNumberFromSectionAsync(sectionId);
        }

        public async Task<Exam?> GetExamFromSection(int sectionId)
        {
            return await serviceProxy.GetExamFromSectionAsync(sectionId);
        }

        public async Task DeleteQuiz(int quizId)
        {
            await serviceProxy.DeleteQuizAsync(quizId);
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            await serviceProxy.UpdateQuizAsync(quiz);
        }

        public async Task<int> CreateQuiz(Quiz quiz)
        {
            await serviceProxy.CreateQuizAsync(quiz);
            return quiz.Id;
        }

        public async Task AddExercisesToQuiz(int quizId, List<Exercise> exercises)
        {
            var exerciseIds = new List<int>();
            foreach (var exercise in exercises)
            {
                exerciseIds.Add(exercise.Id);
            }
            await serviceProxy.AddExercisesToQuizAsync(quizId, exerciseIds);
        }

        public async Task AddExerciseToQuiz(int quizId, int exerciseId)
        {
            await serviceProxy.AddExerciseToQuizAsync(quizId, exerciseId);
        }

        public async Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
        {
            await serviceProxy.RemoveExerciseFromQuizAsync(quizId, exerciseId);
        }

        public async Task DeleteExam(int examId)
        {
            await serviceProxy.DeleteExamAsync(examId);
        }

        public async Task UpdateExam(Exam exam)
        {
            await serviceProxy.UpdateExamAsync(exam);
        }

        public async Task<int> CreateExam(Exam exam)
        {
            await serviceProxy.CreateExamAsync(exam);
            return exam.Id;
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
            await serviceProxy.SubmitQuizAsync(submission);
        }

        public async Task<QuizResult> GetResultAsync(int quizId)
        {
            return await serviceProxy.GetResultAsync(quizId);
        }
    }
}
