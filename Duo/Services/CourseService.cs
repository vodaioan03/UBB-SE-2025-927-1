using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    /// <summary>
    /// Provides core business logic for managing courses, modules, and user interactions via API calls.
    /// </summary>
    public class CourseService : ICourseService
    {
        private readonly ICourseServiceProxy courseServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ICourseServiceProxy"/> class.
        /// </summary>
        public CourseService(ICourseServiceProxy courseServiceProxy)
        {
            this.courseServiceProxy = courseServiceProxy;
        }

        /// <summary>
        /// Retrieves all available courses.
        /// </summary>
        public async Task<List<Course>> GetCoursesAsync()
        {
            try
            {
                return await courseServiceProxy.GetAllCourses();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred while retrieving the courses: " + e);
                return new List<Course>();
            }
        }

        /// <summary>
        /// Retrieves all available tags.
        /// </summary>
        public async Task<List<Tag>> GetTagsAsync()
        {
            try
            {
                return await courseServiceProxy.GetAllTags();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred while retrieving the tags: " + e);
                return new List<Tag>();
            }
        }

        /// <summary>
        /// Gets all tags associated with a specific course.
        /// </summary>
        public async Task<List<Tag>> GetCourseTagsAsync(int courseId)
        {
            try
            {
                return await courseServiceProxy.GetTagsForCourse(courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred while retrieving the course tags: " + e);
                return new List<Tag>();
            }
        }

        /// <summary>
        /// Opens a module for the user if not already opened.
        /// </summary>
        public async Task OpenModuleAsync(int userId, int moduleId)
        {
            try
            {
                if (!await courseServiceProxy.IsModuleOpen(userId, moduleId))
                {
                    await courseServiceProxy.OpenModule(userId, moduleId);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurent while trying to open the module: " + e);
            }
        }

        /// <summary>
        /// Retrieves all modules for a specific course.
        /// </summary>
        public async Task<List<Module>> GetModulesAsync(int courseId)
        {
            try
            {
                return await courseServiceProxy.GetModulesByCourseId(courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred while retrieving modules: {e}");
                return new List<Module>();
            }
        }

        /// <summary>
        /// Retrieves all non-bonus modules for a course.
        /// </summary>
        public async Task<List<Module>> GetNormalModulesAsync(int courseId)
        {
            try
            {
                var modules = await courseServiceProxy.GetModulesByCourseId(courseId);
                return modules.Where(m => !m.IsBonus).ToList();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred while retrieving normal modules: {e}");
                return new List<Module>();
            }
        }

        /// <summary>
        /// Enrolls the user in a course if not already enrolled.
        /// </summary>
        public async Task<bool> EnrollInCourseAsync(int userId, int courseId)
        {
            try
            {
                if (await courseServiceProxy.IsUserEnrolled(userId, courseId))
                {
                    return false;
                }

                await courseServiceProxy.EnrollUser(userId, courseId);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred during enrollment: {e}");
                return false;
            }
        }

        /// <summary>
        /// Completes a module and marks course as completed if all modules are done.
        /// </summary>
        public async Task CompleteModuleAsync(int userId, int moduleId, int courseId)
        {
            try
            {
                await courseServiceProxy.CompleteModule(userId, moduleId);

                if (await courseServiceProxy.IsCourseCompleted(userId, courseId))
                {
                    await courseServiceProxy.MarkCourseAsCompleted(userId, courseId);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred while completing module: {e}");
            }
        }

        /// <summary>
        /// Returns whether the user is enrolled in the specified course.
        /// </summary>
        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
            try
            {
                return await courseServiceProxy.IsUserEnrolled(userId, courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred checking enrollment status: {e}");
                return false;
            }
        }

        /// <summary>
        /// Returns whether the user has completed the specified module.
        /// </summary>
        public async Task<bool> IsModuleCompletedAsync(int userId, int moduleId)
        {
            try
            {
                return await courseServiceProxy.IsModuleCompleted(userId, moduleId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred checking module completion: {e}");
                return false;
            }
        }

        /// <summary>
        /// Filters courses based on search text, type, enrollment status, and tags.
        /// </summary>
        public async Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, List<int> selectedTagIds, int userId)
        {
            try
            {
                var courses = await courseServiceProxy.GetAllCourses();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    courses = courses.Where(c => c.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (filterPremium && filterFree)
                {
                    courses = new List<Course>();
                }
                else if (filterPremium)
                {
                    courses = courses.Where(c => c.IsPremium).ToList();
                }
                else if (filterFree)
                {
                    courses = courses.Where(c => !c.IsPremium).ToList();
                }

                if (filterEnrolled && filterNotEnrolled)
                {
                    courses = new List<Course>();
                }
                else if (filterEnrolled)
                {
                    courses = (await Task.WhenAll(courses.Select(async c => new { c, enrolled = await courseServiceProxy.IsUserEnrolled(userId, c.CourseId) })))
                        .Where(x => x.enrolled)
                        .Select(x => x.c)
                        .ToList();
                }
                else if (filterNotEnrolled)
                {
                    courses = (await Task.WhenAll(courses.Select(async c => new { c, enrolled = await courseServiceProxy.IsUserEnrolled(userId, c.CourseId) })))
                        .Where(x => !x.enrolled)
                        .Select(x => x.c)
                        .ToList();
                }

                if (selectedTagIds.Count > 0)
                {
                    courses = (await Task.WhenAll(courses.Select(async c =>
                    {
                        var tags = await courseServiceProxy.GetTagsForCourse(c.CourseId);
                        return new { c, tags };
                    })))
                    .Where(x => selectedTagIds.All(id => x.tags.Select(t => t.TagId).Contains(id)))
                    .Select(x => x.c)
                    .ToList();
                }

                return courses;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred filtering courses: {e}");
                return new List<Course>();
            }
        }

        /// <summary>
        /// Updates the time the user has spent on a course.
        /// </summary>
        public async Task UpdateTimeSpentAsync(int userId, int courseId, int seconds)
        {
            try
            {
                await courseServiceProxy.UpdateTimeSpent(userId, courseId, seconds);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred updating time spent: {e}");
            }
        }

        /// <summary>
        /// Retrieves the time the user has spent on a course.
        /// </summary>
        public async Task<int> GetTimeSpentAsync(int userId, int courseId)
        {
            try
            {
                return await courseServiceProxy.GetTimeSpent(userId, courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred retrieving time spent: {e}");
                return 0;
            }
        }

        /// <summary>
        /// Handles user interaction with module images.
        /// </summary>
        public async Task<bool> ClickModuleImageAsync(int userId, int moduleId)
        {
            try
            {
                if (await courseServiceProxy.IsModuleImageClicked(userId, moduleId))
                {
                    return false;
                }

                await courseServiceProxy.ClickModuleImage(userId, moduleId);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred clicking module image: {e}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a module is in progress.
        /// </summary>
        public async Task<bool> IsModuleInProgressAsync(int userId, int moduleId)
        {
            try
            {
                return await courseServiceProxy.IsModuleOpen(userId, moduleId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred checking module in progress: {e}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a module is available for the user.
        /// </summary>
        public async Task<bool> IsModuleAvailableAsync(int userId, int moduleId)
        {
            try
            {
                return await courseServiceProxy.IsModuleAvailable(userId, moduleId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred checking module availability: {e}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a course has been completed by the user.
        /// </summary>
        public async Task<bool> IsCourseCompletedAsync(int userId, int courseId)
        {
            try
            {
                return await courseServiceProxy.IsCourseCompleted(userId, courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred checking course completion: {e}");
                return false;
            }
        }

        /// <summary>
        /// Gets the number of completed modules in a course.
        /// </summary>
        public async Task<int> GetCompletedModulesCountAsync(int userId, int courseId)
        {
            try
            {
                return await courseServiceProxy.GetCompletedModulesCount(userId, courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred getting completed modules count: {e}");
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of required modules for a course.
        /// </summary>
        public async Task<int> GetRequiredModulesCountAsync(int courseId)
        {
            try
            {
                return await courseServiceProxy.GetRequiredModulesCount(courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred getting required modules count: {e}");
                return 0;
            }
        }

        /// <summary>
        /// Claims the course completion reward if eligible.
        /// </summary>
        public async Task<bool> ClaimCompletionRewardAsync(int userId, int courseId)
        {
            try
            {
                return await courseServiceProxy.ClaimCompletionReward(userId, courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred claiming completion reward: {e}");
                return false;
            }
        }

        /// <summary>
        /// Claims a reward if the course was completed within a time limit.
        /// </summary>
        public async Task<bool> ClaimTimedRewardAsync(int userId, int courseId, int timeSpent)
        {
            try
            {
                return await courseServiceProxy.ClaimTimedReward(userId, courseId, timeSpent);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred claiming timed reward: {e}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves the time limit for completing a course.
        /// </summary>
        public async Task<int> GetCourseTimeLimitAsync(int courseId)
        {
            try
            {
                return await courseServiceProxy.GetCourseTimeLimit(courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred retrieving course time limit: {e}");
                return 0;
            }
        }

        public async Task<bool> BuyBonusModuleAsync(int userId, int moduleId, int courseId)
        {
            try
            {
                return await courseServiceProxy.BuyBonusModule(userId, moduleId, courseId);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred buying bonus module: {e}");
                return false;
            }
        }
    }
}
