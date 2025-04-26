using System;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents a flashcard-style exercise.
    /// </summary>
    public class FlashcardExercise : Exercise
    {
        /// <summary>
        /// Gets the exercise type.
        /// </summary>
        public string Type { get; } = "Flashcard";

        /// <summary>
        /// Gets the time allowed to complete the exercise in seconds.
        /// </summary>
        public int TimeInSeconds { get; }

        private string answer;

        /// <summary>
        /// Gets or sets the correct answer for the flashcard.
        /// </summary>
        public string Answer
        {
            get => answer;
            set => answer = value;
        }

        private TimeSpan elapsedTime;

        /// <summary>
        /// Gets or sets the elapsed time during the exercise.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get => elapsedTime;
            set => elapsedTime = value;
        }

        /// <summary>
        /// Gets the sentence, mapped from the question, for database support.
        /// </summary>
        public string Sentence => Question;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardExercise"/> class.
        /// </summary>
        /// <param name="id">The exercise ID.</param>
        /// <param name="question">The question text.</param>
        /// <param name="answer">The answer text.</param>
        /// <param name="difficulty">The difficulty level.</param>
        /// <exception cref="ArgumentException">Thrown when the answer is empty or null.</exception>
        public FlashcardExercise(int id, string question, string answer, Difficulty difficulty = Difficulty.Normal)
            : base(id, question, difficulty)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
            }

            this.answer = answer;
            TimeInSeconds = GetDefaultTimeForDifficulty(difficulty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardExercise"/> class with specified time.
        /// </summary>
        /// <param name="id">The exercise ID.</param>
        /// <param name="sentence">The sentence text.</param>
        /// <param name="answer">The answer text.</param>
        /// <param name="timeInSeconds">The allowed time in seconds.</param>
        /// <param name="difficulty">The difficulty level.</param>
        /// <exception cref="ArgumentException">Thrown when the answer is empty or null.</exception>
        public FlashcardExercise(int id, string sentence, string answer, int timeInSeconds, Difficulty difficulty = Difficulty.Normal)
            : base(id, sentence, difficulty)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
            }

            this.answer = answer;
            TimeInSeconds = timeInSeconds;
        }

        /// <summary>
        /// Determines the default time based on the difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level.</param>
        /// <returns>The default time in seconds.</returns>
        private int GetDefaultTimeForDifficulty(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 15,
                Difficulty.Normal => 30,
                Difficulty.Hard => 45,
                _ => 30
            };
        }

        /// <summary>
        /// Gets the correct answer.
        /// </summary>
        /// <returns>The correct answer string.</returns>
        public string GetCorrectAnswer()
        {
            return Answer;
        }

        /// <summary>
        /// Validates the user's answer against the correct answer.
        /// </summary>
        /// <param name="userAnswer">The user's provided answer.</param>
        /// <returns>True if the answer is correct; otherwise, false.</returns>
        public bool ValidateAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
            {
                return false;
            }

            return userAnswer.Trim().Equals(Answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a string representation of the flashcard exercise.
        /// </summary>
        /// <returns>A string with the exercise ID, difficulty, and time limit.</returns>
        public override string ToString()
        {
            return $"Id: {ExerciseId}, Difficulty: {Difficulty}, Time: {TimeInSeconds}s";
        }
    }
}
