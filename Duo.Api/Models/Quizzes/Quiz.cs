namespace Duo.Api.Models.Quizzes;

/// <summary>
/// Represents a concrete quiz implementation with ordering capability.
/// Inherits from BaseQuiz to share common quiz properties and behavior.
/// </summary>
public class Quiz : BaseQuiz
{
    /// <summary>
    /// The order number of the quiz within its section (optional).
    /// Used for sequencing quizzes in a particular order.
    /// </summary>
    public int? OrderNumber { get; set; }

    /// <summary>
    /// Initializes a new instance of the Quiz class.
    /// </summary>
    /// <param name="sectionId">The identifier of the section this quiz belongs to (optional)</param>
    /// <param name="orderNumber">The position of this quiz in the section's ordering (optional)</param>
    public Quiz(int? sectionId, int? orderNumber)
        : base(sectionId)
    {
        OrderNumber = orderNumber;
    }
}
