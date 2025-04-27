using Duo.Api.Models.Sections;
using Duo.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers;

/// <summary>
/// Controller for managing sections.
/// </summary>
[Route("section")]
public class SectionController : BaseController
{
    public SectionController(DataContext dataContext) : base(dataContext)
    {
    }

    /// <summary>
    /// Adds a new section.
    /// </summary>
    /// <param name="section">Section data from form.</param>
    /// <returns>Created section.</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromForm] Section section)
    {
        dataContext.Sections.Add(section);
        await dataContext.SaveChangesAsync();
        return Ok(section);
    }

    /// <summary>
    /// Retrieves a single section by ID.
    /// </summary>
    /// <param name="id">ID of the section.</param>
    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var section = await dataContext.Sections
            .Include(s => s.Quizzes)
            .Include(s => s.Exam)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (section == null)
        {
            return NotFound();
        }
        return Ok(section);
    }

    /// <summary>
    /// Lists all sections.
    /// </summary>
    /// <returns>List of all sections.</returns>
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var section = await dataContext.Sections
            .Include(s => s.Quizzes)
            .Include(s => s.Exam)
            .ToListAsync();
        return Ok(section);
    }

    /// <summary>
    /// Deletes a section by ID.
    /// </summary>
    /// <param name="id">Id of the section to be deleted</param>
    /// <returns>Status of the deletion.</returns>
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var section = await dataContext.Sections.FindAsync(id);
        if (section == null)
        {
            return NotFound();
        }
        dataContext.Sections.Remove(section);
        await dataContext.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Updates an existing section.
    /// </summary>
    /// <param name="section">Updated section data from form</param>
    /// <returns>Status of the update</returns>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromForm] Section section)
    {
        var existingSection = await dataContext.Sections.FindAsync(section.Id);
        if (existingSection == null)
        {
            return NotFound();
        }
        existingSection.SubjectId = section.SubjectId;
        existingSection.Title = section.Title;
        existingSection.Description = section.Description;
        existingSection.RoadmapId = section.RoadmapId;
        existingSection.OrderNumber = section.OrderNumber;
        dataContext.Sections.Update(existingSection);
        await dataContext.SaveChangesAsync();
        return Ok(existingSection);
    }
}

