using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Sections;

namespace Duo.Api.Repositories
{
    public interface IRepository
    {
        #region Users
        Task<List<User>> GetUsersFromDbAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        #endregion

        #region Tags
        Task<List<Tag>> GetTagsFromDbAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        #endregion

        #region Modules
        Task<List<Module>> GetModulesFromDbAsync();
        Task<Module?> GetModuleByIdAsync(int id);
        Task AddModuleAsync(Module module);
        Task UpdateModuleAsync(Module module);
        Task DeleteModuleAsync(int id);
        #endregion

        #region Exercises
        Task<List<Exercise>> GetExercisesFromDbAsync();
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task AddExerciseAsync(Exercise exercise);
        Task UpdateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(int id);
        #endregion

        #region Quizzes
        Task<List<Quiz>> GetQuizzesFromDbAsync();
        Task<Quiz> GetQuizByIdAsync(int id);
        Task AddQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(int id);
        Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId);
        Task<int> CountQuizzesFromSectionAsync(int sectionId);
        Task<int> GetLastOrderNumberFromSectionAsync(int sectionId);
        Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds);
        Task AddExerciseToQuizAsync(int quizId, int exerciseId);
        Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId);
        Task<object> GetQuizResultAsync(int quizId);
        #endregion

        #region Courses
        Task<List<Course>> GetCoursesFromDbAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);


        // here
        Task EnrollUserInCourseAsync(int userId, int courseId);
        Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId);
        Task<bool> IsCourseCompletedAsync(int userId, int courseId);
        Task UpdateTimeSpentAsync(int userId, int courseId, int timeInSeconds);
        Task ClaimCompletionRewardAsync(int userId, int courseId);
        Task ClaimTimeRewardAsync(int userId, int courseId);
        Task<int> GetTimeSpentAsync(int userId, int courseId);
        Task<int> GetCourseTimeLimitAsync(int courseId);
        Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, int userId); // for /get-filtered endpoint
        #endregion

        #region Exams
        Task<List<Exam>> GetExamsFromDbAsync();
        Task<Exam> GetExamByIdAsync(int id);
        Task AddExamAsync(Exam exam);
        Task UpdateExamAsync(Exam exam);
        Task DeleteExamAsync(int id);
        Task<Exam?> GetExamFromSectionAsync(int sectionId);
        Task<List<Exam>> GetAvailableExamsAsync();
        #endregion

        #region Sections
        Task<List<Section>> GetSectionsFromDbAsync();
        Task<Section> GetSectionByIdAsync(int id);
        Task AddSectionAsync(Section section);
        Task UpdateSectionAsync(Section section);
        Task DeleteSectionAsync(int id);
        #endregion

        #region Coins
        Task<int> GetUserCoinBalanceAsync(int userId);
        Task<bool> TryDeductCoinsFromUserWalletAsync(int userId, int cost);
        Task AddCoinsToUserWalletAsync(int userId, int amount);
        Task<DateTime> GetUserLastLoginTimeAsync(int userId);
        Task UpdateUserLastLoginTimeToNowAsync(int userId);
        Task OpenModuleAsync(int userId, int moduleId);
        #endregion
    }
}