using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duo.Models.Quizzes;

namespace Duo.Api.Models.Sections
{
    /// <summary>
    /// Represents a learning section within a roadmap.
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Gets or sets the unique identifier of the section.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the optional subject identifier this section belongs to.
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the title of the section.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the section.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the roadmap identifier this section belongs to.
        /// </summary>
        public int RoadmapId { get; set; }

        /// <summary>
        /// Gets or sets the display order of the section (optional).
        /// </summary>
        public int? OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the list of quizzes associated with this section.
        /// </summary>
        public List<Quiz> Quizzes { get; set; } = new();

        /// <summary>
        /// Gets or sets the final exam associated with this section.
        /// </summary>
        public Exam? Exam { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
        }
    }
}
