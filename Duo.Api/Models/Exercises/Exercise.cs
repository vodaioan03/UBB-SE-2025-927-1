using Duo.Api.Models.Quizzes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents the base class for all exercise types in the system.
    /// Configured for Table-Per-Hierarchy (TPH) inheritance to allow different exercise types
    /// to share a single database table with a discriminator column.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Exercise"/> class.
    /// </remarks>
    /// <param name="ExerciseId">The unique identifier for the exercise.</param>
    /// <param name="question">The question or prompt for the exercise.</param>
    /// <param name="difficulty">The difficulty level of the exercise.</param>
    [Index(nameof(Question))] // Optimizes search queries on the Question field
    public abstract class Exercise(int ExerciseId, string question, Difficulty difficulty)
    {
        /// <summary>
        /// Gets or sets the unique identifier for the exercise.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseId { get; set; } = ExerciseId;

        /// <summary>
        /// Gets or sets the question or prompt for the exercise.
        /// This field is required and indexed for efficient querying.
        /// </summary>
        [Required]
        public string Question { get; set; } = question;

        /// <summary>
        /// Gets or sets the difficulty level of the exercise.
        /// </summary>
        public Difficulty Difficulty { get; set; } = difficulty;

        /// <summary>
        /// Navigation property to the quizzes that include this exercise.
        /// This establishes a many-to-many relationship between exercises and quizzes.
        /// </summary>
        public ICollection<BaseQuiz> Quizzes { get; set; } = [];

        /// <summary>
        /// Returns a string representation of the exercise, including its ID, question, and difficulty level.
        /// </summary>
        /// <returns>A string describing the exercise.</returns>
        public override string ToString()
        {
            return $"Exercise {ExerciseId}: {Question} (Difficulty: {Difficulty})";
        }
    }
}