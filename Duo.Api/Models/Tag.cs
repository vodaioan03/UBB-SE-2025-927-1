using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseApp.Models
{
    /// <summary>
    /// Represents a tag that can be assigned to courses or modules, with support for property change notifications.
    /// </summary>
    public partial class Tag
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
