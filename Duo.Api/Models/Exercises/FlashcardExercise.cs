using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents a flashcard exercise used for studying and memorization.
    /// This class allows defining a flashcard exercise with a question, answer, and time constraints.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FlashcardExercise : Exercise
    {
        #region Constants

        /// <summary>
        /// Represents the type of the exercise as a constant string.
        /// </summary>
        public const string Type = "Flashcard";

        #endregion

        #region Fields

        /// <summary>
        /// Backing field for the <see cref="Answer"/> property.
        /// </summary>
        private string? mAnswer;

        /// <summary>
        /// Backing field for the <see cref="ElapsedTime"/> property.
        /// </summary>
        private TimeSpan mElapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the answer for the flashcard exercise.
        /// </summary>
        public string Answer
        {
            get => mAnswer!;
            set => mAnswer = value;
        }

        /// <summary>
        /// Gets or sets the elapsed time for the flashcard exercise.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get => mElapsedTime;
            set => mElapsedTime = value;
        }

        /// <summary>
        /// Gets the sentence or question for the flashcard exercise.
        /// </summary>
        public string Sentence => Question!;

        /// <summary>
        /// Gets the time allocated for the flashcard exercise in seconds.
        /// </summary>
        public int TimeInSeconds { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardExercise"/> class with the specified parameters.
        /// </summary>
        /// <param name="exerciseId">The unique identifier for the exercise.</param>
        /// <param name="question">The question or prompt for the exercise.</param>
        /// <param name="answer">The correct answer for the exercise.</param>
        /// <param name="difficulty">The difficulty level of the exercise.</param>
        /// <exception cref="ArgumentException">Thrown when the answer is null or empty.</exception>
        public FlashcardExercise(int exerciseId, string question, string answer, Difficulty difficulty = Difficulty.Normal)
            : base(exerciseId, question, difficulty)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty.", nameof(answer));
            }

            mAnswer = answer;
            TimeInSeconds = GetDefaultTimeForDifficulty(difficulty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardExercise"/> class.
        /// This parameterless constructor is required for Entity Framework.
        /// </summary>
        public FlashcardExercise()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardExercise"/> class with additional parameters.
        /// </summary>
        /// <param name="exerciseId">The unique identifier for the exercise.</param>
        /// <param name="sentence">The sentence or question for the exercise.</param>
        /// <param name="answer">The correct answer for the exercise.</param>
        /// <param name="timeInSeconds">The time allocated for the exercise in seconds.</param>
        /// <param name="difficulty">The difficulty level of the exercise.</param>
        /// <exception cref="ArgumentException">Thrown when the answer is null or empty.</exception>
        public FlashcardExercise(int exerciseId, string sentence, string answer, int timeInSeconds, Difficulty difficulty = Difficulty.Normal)
            : base(exerciseId, sentence, difficulty)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty.", nameof(answer));
            }

            mAnswer = answer;
            TimeInSeconds = timeInSeconds;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the correct answer for the flashcard exercise.
        /// </summary>
        /// <returns>The correct answer as a string.</returns>
        public string GetCorrectAnswer()
        {
            return Answer;
        }

        /// <summary>
        /// Validates the user's answer against the correct answer.
        /// </summary>
        /// <param name="userAnswer">The answer provided by the user.</param>
        /// <returns><c>true</c> if the user's answer is correct, otherwise <c>false</c>.</returns>
        public bool ValidateAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
            {
                return false;
            }

            return userAnswer.Trim().Equals(Answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a string representation of the flashcard exercise, including its ID, difficulty, and time.
        /// </summary>
        /// <returns>A string describing the flashcard exercise.</returns>
        public override string ToString()
        {
            return $"Id: {ExerciseId}, Difficulty: {Difficulty}, Time: {TimeInSeconds}s";
        }

        /// <summary>
        /// Determines the default time allocated for the exercise based on its difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level of the exercise.</param>
        /// <returns>The default time in seconds.</returns>
        private static int GetDefaultTimeForDifficulty(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 15,
                Difficulty.Normal => 30,
                Difficulty.Hard => 45,
                _ => 30
            };
        }

        #endregion
    }
}