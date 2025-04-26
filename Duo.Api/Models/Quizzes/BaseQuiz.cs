using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Api.Models.Exercises;

namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents a base class for quiz types.
/// </summary>
public abstract class BaseQuiz
{
    /// <summary>
    /// Gets or sets the quiz ID.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the section ID to which this quiz belongs.
    /// </summary>
    public int? SectionId { get; set; }

    /// <summary>
    /// Gets or sets the exercises associated with this quiz.
    /// </summary>
    public virtual ICollection<Exercise> ExerciseList { get; set; } = new List<Exercise>();

    /// <summary>
    /// Gets or sets the maximum number of exercises allowed in the quiz.
    /// </summary>
    public int MaxExercises { get; set; }

    /// <summary>
    /// Gets or sets the threshold required to pass the quiz.
    /// </summary>
    public double PassingThreshold { get; set; }


}
