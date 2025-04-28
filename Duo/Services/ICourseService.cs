using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models;

namespace Duo.Services
{
    /// <summary>
    /// Interface for course-related operations.
    /// </summary>
    public interface ICourseService
    {
        // Course operations
        Task<List<Course>> GetCoursesAsync();
        Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree,
                                      bool filterEnrolled, bool filterNotEnrolled, List<int> selectedTagIds, int userId);

        // Module operations
        Task<List<Module>> GetModulesAsync(int courseId);
        Task<List<Module>> GetNormalModulesAsync(int courseId);
        Task OpenModuleAsync(int userId, int moduleId);
        Task CompleteModuleAsync(int userId, int moduleId, int courseId);
        Task<bool> IsModuleAvailableAsync(int userId, int moduleId);
        Task<bool> IsModuleCompletedAsync(int userId, int moduleId);
        Task<bool> IsModuleInProgressAsync(int userId, int moduleId);
        Task<bool> ClickModuleImageAsync(int userId, int moduleId);

        // Enrollment operations
        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task<bool> EnrollInCourseAsync(int userId, int courseId);

        // Progress tracking
        Task UpdateTimeSpentAsync(int userId, int courseId, int seconds);
        Task<int> GetTimeSpentAsync(int userId, int courseId);

        // Completion tracking
        Task<bool> IsCourseCompletedAsync(int userId, int courseId);
        Task<int> GetCompletedModulesCountAsync(int userId, int courseId);
        Task<int> GetRequiredModulesCountAsync(int courseId);

        // Reward operations
        Task<bool> ClaimCompletionRewardAsync(int userId, int courseId);
        Task<bool> ClaimTimedRewardAsync(int userId, int courseId, int timeSpent);
        Task<int> GetCourseTimeLimitAsync(int courseId);

        // Tag operations
        Task<List<Tag>> GetTagsAsync();
        Task<List<Tag>> GetCourseTagsAsync(int courseId);
    }
}
