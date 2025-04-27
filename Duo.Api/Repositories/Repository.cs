using CourseApp.Models;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            context = context;
        }

        // Users
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

        // Tags
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

        // Modules
        public async Task<List<Module>> GetModulesFromDbAsync()
        {
            return await context.Modules.ToListAsync();
        }
        public async Task<Module> GetModuleByIdAsync(int id)
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

        // Exercises
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

        // Quizzes
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

        // Courses
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
    }
}