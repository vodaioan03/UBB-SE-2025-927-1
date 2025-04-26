using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Sections;

namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents a concrete quiz implementation with ordering capability.
/// Inherits from BaseQuiz to share common quiz properties and behavior.
/// </summary>
public class Quiz
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
    /// 
    public Section? Section { get; set; }

    /// <summary>
    /// Gets or sets the order number of the quiz within its section.
    /// This determines the sequence in which quizzes appear.
    /// </summary>
    public int? OrderNumber { get; set; }

    //public ICollection<Exercise> Exercises { get; set; }
}
