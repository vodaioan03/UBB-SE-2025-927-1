using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Api.Models.Exercises;
using Duo.Models.Sections;

namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents a base quiz entity containing common properties and relationships for quizzes.
/// </summary>
public abstract class BaseQuiz
{
    /// The unique identifier for the quiz.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the section this quiz belongs to, if any.
    /// </summary>
    [ForeignKey(nameof(Section))]
    public int? SectionId { get; set; }

    /// <summary>
    /// Navigation property to the section this quiz belongs to.
    /// </summary>
    public Section? Section { get; set; }

    /// <summary>
    /// Collection of exercises included in this quiz.
    /// </summary>
    public ICollection<Exercise> ExerciseList { get; set; } = new List<Exercise>();

    /// <summary>
    /// Discriminator column for Table-per-hierarchy inheritance
    /// </summary>
    [Column("QuizType")]
    public string Discriminator { get; protected set; } = null!;

    /// <summary>
    /// Initializes a new instance of the BaseQuiz class.
    /// </summary>
    /// <param name="sectionId">The identifier of the section this quiz belongs to (optional)</param>
    protected BaseQuiz(int? sectionId)
    {
        SectionId = sectionId;
    }
}
