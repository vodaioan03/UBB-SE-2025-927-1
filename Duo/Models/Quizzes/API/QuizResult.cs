// result from the server
public class QuizResult
{
    public int QuizId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public TimeSpan TimeTaken { get; set; }
}
