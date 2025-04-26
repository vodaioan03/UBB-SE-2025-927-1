using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Api.Models;
using Duo.Api.Models.Sections;

namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents a concrete quiz implementation with ordering capability.
/// Inherits from BaseQuiz to share common quiz properties and behavior.
/// </summary>
public class Quiz
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? SectionId { get; set; }
    public Section? Section { get; set; }

    public int? OrderNumber { get; set; }
}
