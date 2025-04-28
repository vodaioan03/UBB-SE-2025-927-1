namespace Duo.Api.DTO
{
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
