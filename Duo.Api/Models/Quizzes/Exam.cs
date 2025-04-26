using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Api.Models.Sections;

namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents an exam, a specialized type of quiz with additional constraints or behaviors.
/// Inherits from BaseQuiz to share common quiz properties and functionality.
/// </summary>
public class Exam
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? SectionId { get; set; }
    public Section? Section { get; set; }
}
