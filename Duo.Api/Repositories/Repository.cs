using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Sections;
using Duo.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly DataContext context;

        public Repository(DataContext dataContext)
        {
            context = dataContext;
        }

        #region Users
        public async Task<List<User>> GetUsersFromDbAsync()
        {
            return await context.Users.ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }
        public async Task AddUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Tags
        public async Task<List<Tag>> GetTagsFromDbAsync()
        {
            return await context.Tags.ToListAsync();
        }
        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await context.Tags.FindAsync(id);
        }
        public async Task AddTagAsync(Tag tag)
        {
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
        }
        public async Task UpdateTagAsync(Tag tag)
        {
            context.Tags.Update(tag);
            await context.SaveChangesAsync();
        }
        public async Task DeleteTagAsync(int id)
        {
            var tag = await context.Tags.FindAsync(id);
            if (tag != null)
            {
                context.Tags.Remove(tag);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Modules
        public async Task<List<Module>> GetModulesFromDbAsync()
        {
            return await context.Modules.ToListAsync();
        }
        public async Task<Module?> GetModuleByIdAsync(int id)
        {
            return await context.Modules.FindAsync(id);
        }
        public async Task AddModuleAsync(Module module)
        {
            context.Modules.Add(module);
            await context.SaveChangesAsync();
        }
        public async Task UpdateModuleAsync(Module module)
        {
            context.Modules.Update(module);
            await context.SaveChangesAsync();
        }
        public async Task DeleteModuleAsync(int id)
        {
            var module = await context.Modules.FindAsync(id);
            if (module != null)
            {
                context.Modules.Remove(module);
                await context.SaveChangesAsync();
            }
        }

        public async Task OpenModuleAsync(int userId, int moduleId)
        {
            var module = await context.Modules.FindAsync(moduleId);

            if (module == null)
            {
                throw new Exception("Module not found");
            }

            context.Modules.Update(module);
            await context.SaveChangesAsync();
        }

        #endregion

        #region Exercises
        public async Task<List<Exercise>> GetExercisesFromDbAsync()
        {
            return await context.Exercises.ToListAsync();
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            return await context.Exercises.FindAsync(id);
        }

        public async Task AddExerciseAsync(Exercise exercise)
        {
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();
        }

        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            context.Exercises.Update(exercise);
            await context.SaveChangesAsync();
        }

        public async Task DeleteExerciseAsync(int id)
        {
            var exercise = await context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                context.Exercises.Remove(exercise);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Quizzes
        public async Task<List<Quiz>> GetQuizzesFromDbAsync()
        {
            return await context.Quizzes.ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await context.Quizzes.FindAsync(id);
        }

        public async Task AddQuizAsync(Quiz quiz)
        {
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            context.Quizzes.Update(quiz);
            await context.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(int id)
        {
            var quiz = await context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                context.Quizzes.Remove(quiz);
                await context.SaveChangesAsync();
            }
        }
        #endregion
        
        #region Courses
        public async Task<List<Course>> GetCoursesFromDbAsync()
        {
            return await context.Courses.ToListAsync();
        }
        
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await context.Courses.FindAsync(id);
        }
        
        public async Task AddCourseAsync(Course course)
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();
        }
        
        public async Task UpdateCourseAsync(Course course)
        {
            context.Courses.Update(course);
            await context.SaveChangesAsync();
        }
        
        public async Task DeleteCourseAsync(int id)
        {
            var course = await context.Courses.FindAsync(id);
            if (course != null)
            {
                context.Courses.Remove(course);
                await context.SaveChangesAsync();
            }
        }


        // ==================

        public async Task EnrollUserInCourseAsync(int userId, int courseId)
        {
            var existing = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            if (existing == null)
            {
                var newCompletion = new CourseCompletion
                {
                    UserId = userId,
                    CourseId = courseId,
                    CompletionRewardClaimed = false,
                    TimedRewardClaimed = false,
                    CompletedAt = DateTime.MinValue // not yet completed
                };
                context.CourseCompletions.Add(newCompletion);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId)
        {
            return await context.CourseCompletions
                .AnyAsync(cc => cc.UserId == userId && cc.CourseId == courseId);
        }

        public async Task<bool> IsCourseCompletedAsync(int userId, int courseId)
        {
            var completion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            return completion != null && completion.CompletedAt != DateTime.MinValue;
        }

        public async Task UpdateTimeSpentAsync(int userId, int courseId, int timeInSeconds)
        {
            var user = await context.Users.FindAsync(userId);
            if (user != null)
            {
                user.NumberOfCompletedSections += timeInSeconds;
                await context.SaveChangesAsync();
            }
        }

        public async Task ClaimCompletionRewardAsync(int userId, int courseId)
        {
            var completion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            if (completion != null && !completion.CompletionRewardClaimed)
            {
                completion.CompletionRewardClaimed = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task ClaimTimeRewardAsync(int userId, int courseId)
        {
            var completion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            if (completion != null && !completion.TimedRewardClaimed)
            {
                completion.TimedRewardClaimed = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTimeSpentAsync(int userId, int courseId)
        {
            var user = await context.Users.FindAsync(userId);
            return user?.NumberOfCompletedSections ?? 0;
        }

        public async Task<int> GetCourseTimeLimitAsync(int courseId)
        {
            var course = await context.Courses.FindAsync(courseId);
            return course?.TimeToComplete ?? 0;
        }

        public async Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, int userId)
        {
            var courses = context.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                courses = courses.Where(c => c.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            if (filterPremium && filterFree)
            {
                // No filter (show all)
            }
            else if (filterPremium)
            {
                courses = courses.Where(c => c.IsPremium);
            }
            else if (filterFree)
            {
                courses = courses.Where(c => !c.IsPremium);
            }

            if (filterEnrolled && filterNotEnrolled)
            {
                // No filter (show all)
            }
            else if (filterEnrolled)
            {
                // User is enrolled in the course
                courses = courses.Where(c => context.CourseCompletions.Any(cc => cc.UserId == userId && cc.CourseId == c.CourseId));
            }
            else if (filterNotEnrolled)
            {
                // User is not enrolled in the course
                courses = courses.Where(c => !context.CourseCompletions.Any(cc => cc.UserId == userId && cc.CourseId == c.CourseId));
            }

            return await courses.ToListAsync();
        }



        #endregion

        #region Exams
        public async Task<List<Exam>> GetExamsFromDbAsync()
        {
            return await context.Exams.ToListAsync();
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            return await context.Exams.FindAsync(id);
        }

        public async Task AddExamAsync(Exam exam)
        {
            context.Exams.Add(exam);
            await context.SaveChangesAsync();
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            context.Exams.Update(exam);
            await context.SaveChangesAsync();
        }

        public async Task DeleteExamAsync(int id)
        {
            var exam = await context.Exams.FindAsync(id);
            if (exam != null)
            {
                context.Exams.Remove(exam);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Exam?> GetExamFromSectionAsync(int sectionId)
        {
            return await context.Exams
                .Include(e => e.Exercises)
                .FirstOrDefaultAsync(e => e.SectionId == sectionId);
        }

        public async Task<List<Exam>> GetAvailableExamsAsync()
        {
            return await context.Exams
                .Include(e => e.Exercises)
                .Where(e => e.SectionId == null)
                .ToListAsync();
        }
        #endregion

        #region Sections
        public async Task<List<Section>> GetSectionsFromDbAsync()
        {
            return await context.Sections.ToListAsync();
        }
        public async Task<Section> GetSectionByIdAsync(int id)
        {
            return await context.Sections.FindAsync(id);
        }
        public async Task AddSectionAsync(Section section)
        {
            context.Sections.Add(section);
            await context.SaveChangesAsync();
        }
        public async Task UpdateSectionAsync(Section section)
        {
            context.Sections.Update(section);
            await context.SaveChangesAsync();
        }
        public async Task DeleteSectionAsync(int id)
        {
            var section = await context.Sections.FindAsync(id);
            if (section != null)
            {
                context.Sections.Remove(section);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Coins

        public async Task<int> GetUserCoinBalanceAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user?.CoinBalance ?? 0;
        }

        public async Task<bool> TryDeductCoinsFromUserWalletAsync(int userId, int cost)
        {
            var user = await context.Users.FindAsync(userId);
            if (user != null && user.CoinBalance >= cost)
            {
                user.CoinBalance -= cost;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AddCoinsToUserWalletAsync(int userId, int amount)
        {
            var user = await context.Users.FindAsync(userId);
            if (user != null)
            {
                user.CoinBalance += amount;
                await context.SaveChangesAsync();
            }
        }

        public async Task<DateTime> GetUserLastLoginTimeAsync(int userId)
        {
            if (context == null)
            {
                throw new Exception("Database context is not initialized.");
            }

            // Ensure we're awaiting the database call asynchronously.
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            // If the user is not found, throw a custom exception.
            if (user == null)
            {
                throw new Exception($"User with ID {userId} not found");
            }

            // If the user exists, return the LastLoginTime.
            return user.LastLoginTime;
        }

        public async Task UpdateUserLastLoginTimeToNowAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginTime = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        #endregion
    }
}