using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents a fill-in-the-blank exercise.
    /// This class allows for defining a fill-in-the-blank type exercise with a list of possible correct answers.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FillInTheBlankExercise : Exercise
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of possible correct answers for this fill-in-the-blank exercise.
        /// </summary>
        public List<string>? PossibleCorrectAnswers { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FillInTheBlankExercise"/> class with the specified parameters.
        /// </summary>
        /// <param name="exerciseId">The unique identifier for the exercise.</param>
        /// <param name="question">The question or prompt for the exercise.</param>
        /// <param name="difficulty">The difficulty level of the exercise.</param>
        /// <param name="possibleCorrectAnswers">A list of possible correct answers for this exercise.</param>
        /// <exception cref="ArgumentException">Thrown when the list of possible correct answers is null or empty.</exception>
        public FillInTheBlankExercise(int exerciseId, string question, Difficulty difficulty, List<string> possibleCorrectAnswers)
            : base(exerciseId, question, difficulty)
        {
            if (possibleCorrectAnswers == null || possibleCorrectAnswers.Count == 0)
            {
                throw new ArgumentException("Answers cannot be empty.", nameof(possibleCorrectAnswers));
            }

            PossibleCorrectAnswers = possibleCorrectAnswers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FillInTheBlankExercise"/> class.
        /// This parameterless constructor is required for Entity Framework.
        /// </summary>
        public FillInTheBlankExercise()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the type of the exercise.
        /// </summary>
        public static ExerciseType ExerciseType => ExerciseType.FillInTheBlank;

        /// <summary>
        /// Validates the user's answers against the possible correct answers.
        /// </summary>
        /// <param name="userAnswers">A list of answers provided by the user.</param>
        /// <returns><c>true</c> if the answers are correct, otherwise <c>false</c>.</returns>
        public bool ValidateAnswer(List<string> userAnswers)
        {
            if (userAnswers == null || userAnswers.Count != PossibleCorrectAnswers!.Count)
            {
                return false;
            }

            // Check if each user answer matches the corresponding correct answer
            for (int i = 0; i < PossibleCorrectAnswers.Count; i++)
            {
                if (!string.Equals(userAnswers[i].Trim(), PossibleCorrectAnswers[i].Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string representation of the fill-in-the-blank exercise, including its question, difficulty, and possible correct answers.
        /// </summary>
        /// <returns>A string describing the fill-in-the-blank exercise.</returns>
        public override string ToString()
        {
            var answers = string.Join(", ", PossibleCorrectAnswers!);
            return $"{base.ToString()} [Fill in the Blank] Correct Answers: {answers}";
        }

        #endregion
    }
}
