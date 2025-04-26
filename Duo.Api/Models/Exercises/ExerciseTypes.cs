using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Models.Exercises;

namespace Duo.Api.Models.Exercises;

/// <summary>
/// Represents a type/category of an exercise (e.g., "Association", "Fill in the blank").
/// </summary>
public class ExerciseType
{
    /// <summary>
    /// The unique identifier of the exercise type.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// The name of the exercise type.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Exercises linked to this exercise type.
    /// </summary>
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
