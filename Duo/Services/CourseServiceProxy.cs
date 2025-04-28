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

        public async Task<List<Course>> GetAllCourses()
        {
            return await httpClient.GetFromJsonAsync<List<Course>>("course/list");
        }

        public async Task<Course> GetCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<Course>($"course/get?id={courseId}");
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>("tag/list");
        }

        public async Task<List<Tag>> GetTagsForCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"course/{courseId}/tags");
        }

        public async Task OpenModule(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync("module/open", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<List<Module>> GetModulesByCourseId(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Module>>($"module/list?courseId={courseId}");
        }

        public async Task<Module> GetModule(int moduleId)
        {
            return await httpClient.GetFromJsonAsync<Module>($"module/get?id={moduleId}");
        }

        public async Task<bool> IsModuleOpen(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/isOpen?userId={userId}&moduleId={moduleId}");
        }

        public async Task EnrollUser(int userId, int courseId)
        {
            await httpClient.PostAsJsonAsync("course/enroll", new { UserId = userId, CourseId = courseId });
        }

        public async Task<bool> IsUserEnrolled(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"course/isEnrolled?userId={userId}&courseId={courseId}");
        }

        public async Task CompleteModule(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync("module/complete", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<bool> IsCourseCompleted(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"course/isCompleted?userId={userId}&courseId={courseId}");
        }

        public async Task MarkCourseAsCompleted(int userId, int courseId)
        {
            await httpClient.PostAsJsonAsync("course/complete", new { UserId = userId, CourseId = courseId });
        }

        public async Task UpdateTimeSpent(int userId, int courseId, int seconds)
        {
            await httpClient.PostAsJsonAsync("course/updateTime", new { UserId = userId, CourseId = courseId, Seconds = seconds });
        }

        public async Task<int> GetTimeSpent(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/timeSpent?userId={userId}&courseId={courseId}");
        }

        public async Task ClickModuleImage(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync("module/clickImage", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<bool> IsModuleImageClicked(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/imageClicked?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleAvailable(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/isAvailable?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleCompleted(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"module/isCompleted?userId={userId}&moduleId={moduleId}");
        }

        public async Task<int> GetCompletedModulesCount(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/completedModules?userId={userId}&courseId={courseId}");
        }

        public async Task<int> GetRequiredModulesCount(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/requiredModules?courseId={courseId}");
        }

        public async Task<bool> ClaimCompletionReward(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync("course/claimReward", new { UserId = userId, CourseId = courseId });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ClaimTimedReward(int userId, int courseId, int timeSpent)
        {
            var response = await httpClient.PostAsJsonAsync("course/claimTimedReward", new { UserId = userId, CourseId = courseId, TimeSpent = timeSpent });
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetCourseTimeLimit(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"course/timeLimit?courseId={courseId}");
        }
    }
}
