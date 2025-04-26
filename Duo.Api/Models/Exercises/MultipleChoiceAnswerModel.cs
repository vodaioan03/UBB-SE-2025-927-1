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

    }
}
