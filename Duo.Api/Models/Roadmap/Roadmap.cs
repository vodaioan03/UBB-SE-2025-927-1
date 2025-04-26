using Duo.Api.Models.Sections;
using System.ComponentModel.DataAnnotations;

namespace Duo.Models.Api.Roadmap;

/// <summary>
/// Represents a roadmap consisting of multiple sections.
/// </summary>
public class Roadmap
{
    /// <summary>
    /// Gets or sets the unique identifier for the roadmap.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the roadmap.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sections associated with this roadmap.
    /// </summary>
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

}
