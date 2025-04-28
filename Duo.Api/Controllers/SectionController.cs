using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Api.DTO.Requests;
using Duo.Api.Models.Sections;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    [Route("section")]
    public class SectionController : BaseController
    {
        public SectionController(IRepository repository) : base(repository)
        {
        }

        private async Task<int> GetLastOrderNumberAsync(int roadmapId)
        {
            List<Section> sections = await repository.GetSectionsFromDbAsync();
            List<int> orderNumbers = sections
                        .Where(section => section.RoadmapId == roadmapId)
                        .OrderBy(section => section.OrderNumber)
                        .Select(section => section.OrderNumber ?? 0)
                        .ToList();

            if (orderNumbers.Count == 0)
            {
                return 1;
            }

            for (int i = 1; i <= orderNumbers.Count; i++)
            {
                if (orderNumbers[i - 1] != i)
                {
                    return i;
                }
            }

            return orderNumbers.Count + 1;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSection([FromForm] AddSectionRequest request)
        {
            try
            {
                Section section = new Section
                {
                    SubjectId = request.SubjectId,
                    Title = request.Title,
                    Description = request.Description,
                    RoadmapId = request.RoadmapId,
                    OrderNumber = request.OrderNumber ?? await this.GetLastOrderNumberAsync(request.RoadmapId)
                    // Quizzes and Exam are left as null
                };

                await repository.AddSectionAsync(section);

                return Ok(new { message = "Section added successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSection([FromForm] int id)
        {
            try
            {
                var section = await repository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found!" });
                }

                await repository.DeleteSectionAsync(id);
                return Ok(new { message = "Section removed successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSectionById(int id)
        {
            try
            {
                var section = await repository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found!" });
                }

                return Ok(new { result = section, message = "Successfully got section!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListSections()
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                return Ok(new { result = sections, message = "Successfully got list of sections" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("list/roadmap/{roadmapId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListSectionsByRoadmapId(int roadmapId)
        {
            try
            {
                List<Section> sections = await repository.GetSectionsFromDbAsync();
                sections = sections
                            .Where(section => section.RoadmapId == roadmapId)
                            .ToList();
                if (!sections.Any())
                {
                    return NotFound(new { result = new List<Section>(), message = "No sections found for the specified roadmap!" });
                }
                return Ok(new { result = sections, message = "Successfully got list of sections" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSection([FromForm] int id, [FromForm] UpdateSectionRequest request)
        {
            try
            {
                var section = await repository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found!" });
                }

                section.SubjectId = request.SubjectId ?? section.SubjectId;
                section.Title = request.Title ?? section.Title;
                section.Description = request.Description ?? section.Description;
                section.RoadmapId = request.RoadmapId ?? section.RoadmapId;
                section.OrderNumber = request.OrderNumber ?? section.OrderNumber;

                await repository.UpdateSectionAsync(section);
                return Ok(new { message = "Section updated successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}