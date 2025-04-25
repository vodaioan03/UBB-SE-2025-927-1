using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Duo.Api.Models.Sections;

namespace Duo.Api.Models.Roadmap;

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

    /// <summary>
    /// Returns a string representation of the roadmap.
    /// </summary>
    public override string ToString()
    {
        return $"Roadmap {Id}: {Name} - {Sections.Count} sections";
    }
}
