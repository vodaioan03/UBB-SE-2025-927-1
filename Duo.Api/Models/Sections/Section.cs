using System.Collections.Generic;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Roadmaps;

namespace Duo.Api.Models.Sections;

public class Section
{
    public int Id { get; set; }
    public int? SubjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoadmapId { get; set; }
    public int? OrderNumber { get; set; }

    public ICollection<Quiz> Quizzes { get; set; }

    public Exam? Exam { get; set; }
    public Roadmap Roadmap { get; internal set; }
}
