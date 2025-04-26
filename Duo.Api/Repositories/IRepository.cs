using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;

namespace Duo.Api.Repsitories
{
    public interface  IRepository
    {
        // Exercises
        Task<List<Exercise>> GetExercisesFromDbAsync();
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task AddExerciseAsync(Exercise exercise);
        Task UpdateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(int id);

        //Quizzes
        Task<List<BaseQuiz>> GetQuizzesFromDbAsync();
        Task<BaseQuiz> GetQuizByIdAsync(int id);
        Task AddQuizAsync(BaseQuiz quiz);
        Task UpdateQuizAsync(BaseQuiz quiz);
        Task DeleteQuizAsync(int id);
    }
}