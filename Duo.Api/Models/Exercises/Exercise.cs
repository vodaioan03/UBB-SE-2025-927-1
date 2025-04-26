using Duo.Models.Quizzes;
using System;

namespace Duo.Models.Exercises;

public abstract class Exercise
{
    public int Id { get; set; }
    public string Question { get; set; }
    public Difficulty Difficulty { get; set; }

    /// <summary>
    /// Navigation property to the quizzes containing this exercise.
    /// </summary>
    public ICollection<BaseQuiz> Quizzes { get; set; } = new List<BaseQuiz>();

    protected Exercise(int id, string question, Difficulty difficulty)
    {
        Id = id;
        Question = question;
        Difficulty = difficulty;
    }

    public override string ToString()
    {
        return $"Exercise {Id}: {Question} (Difficulty: {Difficulty})";
    }
}