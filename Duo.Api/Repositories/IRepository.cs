using CourseApp.Models;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;

namespace Duo.Api.Repositories
{
    public interface IRepository
    {
        // Users
        Task<List<User>> GetUsersFromDbAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        // Tags
        Task<List<Tag>> GetTagsFromDbAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);

        // Modules
        Task<List<Module>> GetModulesFromDbAsync();
        Task<Module?> GetModuleByIdAsync(int id);
        Task AddModuleAsync(Module module);
        Task UpdateModuleAsync(Module module);
        Task DeleteModuleAsync(int id);

        // Exercises
        Task<List<Exercise>> GetExercisesFromDbAsync();
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task AddExerciseAsync(Exercise exercise);
        Task UpdateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(int id);

        //Quizzes
        Task<List<Quiz>> GetQuizzesFromDbAsync();
        Task<Quiz> GetQuizByIdAsync(int id);
        Task AddQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(int id);
    }
}