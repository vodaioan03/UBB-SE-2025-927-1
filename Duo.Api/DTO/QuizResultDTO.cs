using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.DTO
{
    [ExcludeFromCodeCoverage]
    public class QuizResultDTO
    {
        public int QuizId { get; set; }
        public int ExercisesCount { get; set; }

        public QuizResultDTO(int quizId, int exercisesCount)
        {
            QuizId = quizId;
            ExercisesCount = exercisesCount;
        }
    }
}
