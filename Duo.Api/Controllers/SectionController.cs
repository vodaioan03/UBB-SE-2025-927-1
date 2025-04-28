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
            var sections = await repository.GetSectionsFromDbAsync();
            var orderNumbers = sections
                        .Where(section => section.RoadmapId == roadmapId)
                        .OrderBy(section => section.OrderNumber)
                        .Select(section => section.OrderNumber ?? 0)
                        .ToList();

            if (!orderNumbers.Any())
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
                var sections = await repository.GetSectionsFromDbAsync();
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

        [HttpGet("by-roadmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSectionsByRoadmap([FromQuery] int roadmapId)
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                var filtered = sections.Where(s => s.RoadmapId == roadmapId).ToList();
                return Ok(new { result = filtered, message = "Successfully got sections by roadmap" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("count-on-roadmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSectionCountOnRoadmap([FromQuery] int roadmapId)
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                var count = sections.Count(s => s.RoadmapId == roadmapId);
                return Ok(new { result = count, message = "Successfully counted sections" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("last-from-roadmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastOrderNumberFromRoadmap([FromQuery] int roadmapId)
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                var sectionsInRoadmap = sections.Where(s => s.RoadmapId == roadmapId);
                var lastOrderNumber = sectionsInRoadmap.Any()
                    ? sectionsInRoadmap.Max(s => s.OrderNumber)
                    : 0;
                return Ok(new { result = lastOrderNumber, message = "Successfully got last order number" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}