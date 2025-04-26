using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;

namespace Duo.Api.Repsitories
{
    public interface  IRepository
    {
        Task<List<Exercise>> GetExercisesFromDbAsync();
         
        Task<List<BaseQuiz>> GetQuizzesFromDbAsync();
    }
}