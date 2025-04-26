using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Sections;

namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents an exam, a specialized type of quiz with additional constraints or behaviors.
/// Inherits from BaseQuiz to share common quiz properties and functionality.
/// </summary>
public class Exam
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Unique identifier of the section this quiz belongs to.
    /// This is a foreign key relationship.
    /// </summary>
    public int? SectionId { get; set; }

    /// <summary>
    /// Navigation property to the section this quiz belongs to.
    /// </summary>
    public Section? Section { get; set; }

    //public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
