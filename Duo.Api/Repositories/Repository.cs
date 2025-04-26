using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Repsitories
{
    public class Repository : IRepository
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            context = context;
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
        public async Task<List<BaseQuiz>> GetQuizzesFromDbAsync()
        {
            return await context.Quizzes.ToListAsync();
        }

        public async Task<BaseQuiz> GetQuizByIdAsync(int id)
        {
            return await context.Quizzes.FindAsync(id);
        }

        public async Task AddQuizAsync(BaseQuiz quiz)
        {
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();
        }

        public async Task UpdateQuizAsync(BaseQuiz quiz)
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
    }
}