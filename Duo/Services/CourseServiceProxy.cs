using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models;

namespace Duo.Services
{
    public class CourseServiceProxy
    {
        private readonly HttpClient httpClient;

        public CourseServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Course>>("course/list");
        }

        public async Task<Course> GetCourseAsync(int courseId)
        {
            return await httpClient.GetFromJsonAsync<Course>($"course/get?id={courseId}");
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>("tag/list");
        }

        public async Task<List<Tag>> GetTagsForCourseAsync(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"course/{courseId}/tags");
        }

        public async Task OpenModuleAsync(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync("module/open", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<List<Module>> GetModulesByCourseIdAsync(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Module>>($"module/list?courseId={courseId}");
        }

        public async Task<Module> GetModuleAsync(int moduleId)
        {
            return await httpClient.GetFromJsonAsync<Module>($"module/get?id={moduleId}");
        }

        public async Task<bool> IsModuleOpenAsync(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/isOpen?userId={userId}&moduleId={moduleId}");
        }

        public async Task EnrollUserAsync(int userId, int courseId)
        {
            await httpClient.PostAsJsonAsync("course/enroll", new { UserId = userId, CourseId = courseId });
        }

        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"course/isEnrolled?userId={userId}&courseId={courseId}");
        }

        public async Task CompleteModuleAsync(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync("module/complete", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<bool> IsCourseCompletedAsync(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"course/isCompleted?userId={userId}&courseId={courseId}");
        }

        public async Task MarkCourseAsCompletedAsync(int userId, int courseId)
        {
            await httpClient.PostAsJsonAsync("course/complete", new { UserId = userId, CourseId = courseId });
        }

        public async Task UpdateTimeSpentAsync(int userId, int courseId, int seconds)
        {
            await httpClient.PostAsJsonAsync("course/updateTime", new { UserId = userId, CourseId = courseId, Seconds = seconds });
        }

        public async Task<int> GetTimeSpentAsync(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/timeSpent?userId={userId}&courseId={courseId}");
        }

        public async Task ClickModuleImageAsync(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync("module/clickImage", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<bool> IsModuleImageClickedAsync(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/imageClicked?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleAvailableAsync(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/isAvailable?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleCompletedAsync(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/isCompleted?userId={userId}&moduleId={moduleId}");
        }

        public async Task<int> GetCompletedModulesCountAsync(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/completedModules?userId={userId}&courseId={courseId}");
        }

        public async Task<int> GetRequiredModulesCountAsync(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/requiredModules?courseId={courseId}");
        }

        public async Task<bool> ClaimCompletionRewardAsync(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync("course/claimReward", new { UserId = userId, CourseId = courseId });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ClaimTimedRewardAsync(int userId, int courseId, int timeSpent)
        {
            var response = await httpClient.PostAsJsonAsync("course/claimTimedReward", new { UserId = userId, CourseId = courseId, TimeSpent = timeSpent });
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetCourseTimeLimitAsync(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/timeLimit?courseId={courseId}");
        }
    }
}
