namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Enum representing the different types of exercises in the system.
    /// This enum helps categorize exercises by their type.
    /// </summary>
    public enum ExerciseType
    {
        Association,
        FillInTheBlank,
        MultipleChoice,
        Flashcard
    }

    /// <summary>
    /// A static class that contains predefined exercise types as strings.
    /// This is used to quickly access a list of available exercise types in the system.
    /// </summary>
    public static class ExerciseTypes
    {
        /// <summary>
        /// List of available exercise types represented as strings.
        /// </summary>
        public static readonly List<string> EXERCISE_TYPES = new()
        {
            "Association",
            "Fill in the Blank",
            "Multiple Choice",
            "Flashcard"
        };
    }
}
