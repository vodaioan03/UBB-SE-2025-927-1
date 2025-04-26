using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents an association exercise where users match items from two lists.
    /// Inherits from the <see cref="Exercise"/> base class.
    /// Configured for Table-Per-Hierarchy (TPH) inheritance.
    /// </summary>
    [Index(nameof(Question))] // Ensures efficient search queries on the Question field
    public class AssociationExercise : Exercise
    {
        /// <summary>
        /// Gets or sets the first list of answers for the association exercise.
        /// </summary>
        [Required]
        public List<string> FirstAnswersList { get; set; } = [];

        /// <summary>
        /// Gets or sets the second list of answers for the association exercise.
        /// </summary>
        [Required]
        public List<string> SecondAnswersList { get; set; } = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociationExercise"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the exercise.</param>
        /// <param name="question">The question or prompt for the exercise.</param>
        /// <param name="difficulty">The difficulty level of the exercise.</param>
        /// <param name="firstAnswers">The first list of answers for the exercise.</param>
        /// <param name="secondAnswers">The second list of answers for the exercise.</param>
        /// <exception cref="ArgumentException">Thrown if the answer lists are null or have different lengths.</exception>
        public AssociationExercise(
            int id,
            string question,
            Difficulty difficulty,
            List<string> firstAnswers,
            List<string> secondAnswers)
            : base(id, question, difficulty)
        {
            if (firstAnswers == null || secondAnswers == null || firstAnswers.Count != secondAnswers.Count)
            {
                throw new ArgumentException("Answer lists must have the same length.");
            }

            FirstAnswersList = firstAnswers;
            SecondAnswersList = secondAnswers;
        }

        /// <summary>
        /// Validates the user's answer by checking if the provided pairs match the correct associations.
        /// </summary>
        /// <param name="userPairs">The list of user-provided pairs to validate.</param>
        /// <returns>True if all pairs are correct; otherwise, false.</returns>
        public bool ValidateAnswer(List<(string, string)> userPairs)
        {
            if (userPairs == null || userPairs.Count != FirstAnswersList.Count)
            {
                return false;
            }

            foreach (var (userA, userB) in userPairs)
            {
                int index = FirstAnswersList.IndexOf(userA);
                if (index == -1 || SecondAnswersList[index] != userB)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string representation of the association exercise, including its ID, question, and answer pairs.
        /// </summary>
        /// <returns>A string describing the association exercise.</returns>
        public override string ToString()
        {
            var pairs = string.Join(", ", FirstAnswersList.Zip(SecondAnswersList, (a, b) => $"{a} ↔ {b}"));
            return $"{base.ToString()} [Association] Pairs: {pairs}";
        }
    }
}