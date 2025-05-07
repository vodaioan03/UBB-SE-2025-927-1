using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    public class CourseServiceProxy : ICourseServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174";

        public CourseServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await httpClient.GetFromJsonAsync<List<Course>>($"{url}/api/course/list");
        }

        public async Task<Course> GetCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<Course>($"{url}/api/course/get?id={courseId}");
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"{url}/api/tag/list");
        }

        public async Task<List<Tag>> GetTagsForCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"{url}/api/course/{courseId}/tags");
        }

        public async Task OpenModule(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync($"{url}/api/module/open", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<List<Module>> GetModulesByCourseId(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Module>>($"{url}/api/module/list?courseId={courseId}");
        }

        public async Task<Module> GetModule(int moduleId)
        {
            return await httpClient.GetFromJsonAsync<Module>($"{url}/api/module/get?id={moduleId}");
        }

        public async Task<bool> IsModuleOpen(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/module/isOpen?userId={userId}&moduleId={moduleId}");
        }

        public async Task EnrollUser(int userId, int courseId)
        {
            await httpClient.PostAsJsonAsync($"{url}/api/course/enroll", new { UserId = userId, CourseId = courseId });
        }

        public async Task<bool> IsUserEnrolled(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/course/isEnrolled?userId={userId}&courseId={courseId}");
        }

        public async Task CompleteModule(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync($"{url}/api/module/complete", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<bool> IsCourseCompleted(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/course/isCompleted?userId={userId}&courseId={courseId}");
        }

        public async Task MarkCourseAsCompleted(int userId, int courseId)
        {
            await httpClient.PostAsJsonAsync($"{url}/course/complete", new { UserId = userId, CourseId = courseId });
        }

        public async Task UpdateTimeSpent(int userId, int courseId, int seconds)
        {
            await httpClient.PostAsJsonAsync($"{url}/api/course/updateTime", new { UserId = userId, CourseId = courseId, Seconds = seconds });
        }

        public async Task<int> GetTimeSpent(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/course/timeSpent?userId={userId}&courseId={courseId}");
        }

        public async Task ClickModuleImage(int userId, int moduleId)
        {
            await httpClient.PostAsJsonAsync($"{url}/api/module/clickImage", new { UserId = userId, ModuleId = moduleId });
        }

        public async Task<bool> IsModuleImageClicked(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/module/imageClicked?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleAvailable(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/module/isAvailable?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleCompleted(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/module/isCompleted?userId={userId}&moduleId={moduleId}");
        }

        public async Task<int> GetCompletedModulesCount(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/course/completedModules?userId={userId}&courseId={courseId}");
        }

        public async Task<int> GetRequiredModulesCount(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/course/requiredModules?courseId={courseId}");
        }

        public async Task<bool> ClaimCompletionReward(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/course/claimReward", new { UserId = userId, CourseId = courseId });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ClaimTimedReward(int userId, int courseId, int timeSpent)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/course/claimTimedReward", new { UserId = userId, CourseId = courseId, TimeSpent = timeSpent });
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetCourseTimeLimit(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/course/timeLimit?courseId={courseId}");
        }

        public async Task<bool> BuyBonusModule(int userId, int moduleId, int courseId)
        {
            var requestContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    CourseId = courseId
                }),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync($"{url}/api/course/buyBonusModule", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}
