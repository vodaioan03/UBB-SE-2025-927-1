using System.ComponentModel.DataAnnotations;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents an answer option for a multiple-choice exercise.
    /// </summary>
    public class MultipleChoiceAnswerModel
    {
        /// <summary>
        /// Gets or sets the answer text. Required field.
        /// </summary>
        [Required]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the answer is correct.
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Parameterless constructor required for Entity Framework.
        /// </summary>
        public MultipleChoiceAnswerModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleChoiceAnswerModel"/> class.
        /// </summary>
        /// <param name="answer">The answer text.</param>
        /// <param name="isCorrect">Indicates if the answer is correct.</param>
        /// <exception cref="ArgumentException">Thrown when answer is null or whitespace.</exception>
        public MultipleChoiceAnswerModel(string answer, bool isCorrect)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
            }

            Answer = answer;
            IsCorrect = isCorrect;
        }

        /// <summary>
        /// Returns a string representation of the answer, indicating correctness.
        /// </summary>
        /// <returns>A string representing the answer and its correctness.</returns>
        public override string ToString()
        {
            return $"{Answer}{(IsCorrect ? " (Correct)" : string.Empty)}";
        }
    }
}
