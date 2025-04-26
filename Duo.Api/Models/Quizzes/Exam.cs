namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents an exam, a specialized type of quiz with additional constraints or behaviors.
/// Inherits from BaseQuiz to share common quiz properties and functionality.
/// </summary>
public class Exam : BaseQuiz
{
    /// <summary>
    /// Initializes a new instance of the Exam class.
    /// </summary>
    /// <param name="sectionId">The identifier of the section this exam belongs to (optional)</param>
    public Exam(int? sectionId)
        : base(sectionId)
    {
    }
}
