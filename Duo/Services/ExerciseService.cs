using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;

namespace Duo.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseService exerciseServiceProxy;

        public ExerciseService(IExerciseService exerciseServiceProxy)
        {
            this.exerciseServiceProxy = exerciseServiceProxy ?? throw new ArgumentNullException(nameof(exerciseServiceProxy));
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            try
            {
                return await exerciseServiceProxy.GetAllExercises();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching all exercises: {ex.Message}");
                return new List<Exercise>();
            }
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            try
            {
                return await exerciseServiceProxy.GetExerciseById(exerciseId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching exercise by ID {exerciseId}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            try
            {
                return await exerciseServiceProxy.GetAllExercisesFromQuiz(quizId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching exercises from quiz ID {quizId}: {ex.Message}");
                return new List<Exercise>();
            }
        }
        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            try
            {
                return await exerciseServiceProxy.GetAllExercisesFromExam(examId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching exercises from exam ID {examId}: {ex.Message}");
                return new List<Exercise>();
            }
        }

        public async Task DeleteExercise(int exerciseId)
        {
            try
            {
                await exerciseServiceProxy.DeleteExercise(exerciseId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting exercise ID {exerciseId}: {ex.Message}");
            }
        }

        public async Task CreateExercise(Exercise exercise)
        {
            try
            {
                ValidationHelper.ValidateGenericExercise(exercise);
                await exerciseServiceProxy.CreateExercise(exercise);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating exercise: {ex.Message}");
            }
        }
    }
}
