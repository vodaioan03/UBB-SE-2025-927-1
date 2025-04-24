namespace Duo.Models.Quizzes.API
{
    // for submitting answers
    public class AnswerSubmission
    {
        public int QuestionId { get; set; }
        public int SelectedOptionIndex { get; set; }
    }
}
