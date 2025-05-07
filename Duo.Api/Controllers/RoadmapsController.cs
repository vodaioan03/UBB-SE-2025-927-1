using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class RoadmapsController : ControllerBase
    {
        private readonly DataContext context;

        public RoadmapsController(DataContext context)
        {
            this.context = context;
        }

        // GET: api/Roadmaps
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Roadmap>>> GetRoadmap()
        {
            return await context.Roadmaps.ToListAsync();
        }

        // GET: api/Roadmaps/5
        [HttpGet("get")]
        public async Task<ActionResult<Roadmap>> GetRoadmap(int id)
        {
            var roadmap = await context.Roadmaps.FindAsync(id);

            if (roadmap == null)
            {
                return NotFound();
            }

            return roadmap;
        }

        // GET: api/Roadmaps?name=example
        [HttpGet("get-searched")]
        public async Task<ActionResult<IEnumerable<Roadmap>>> GetRoadmapsByName(string name)
        {
            var roadmaps = await context.Roadmaps
                .Where(r => r.Name.Contains(name))
                .ToListAsync();
            if (roadmaps == null || !roadmaps.Any())
            {
                return NotFound();
            }
            return roadmaps;
        }

        // PUT: api/Roadmaps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("modify")]
        public async Task<IActionResult> PutRoadmap(int id, Roadmap roadmap)
        {
            if (id != roadmap.Id)
            {
                return BadRequest();
            }

            context.Entry(roadmap).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoadmapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Roadmaps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Roadmap>> PostRoadmap(Roadmap roadmap)
        {
            context.Roadmaps.Add(roadmap);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetRoadmap", new { id = roadmap.Id }, roadmap);
        }

        // DELETE: api/Roadmaps/5
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRoadmap(int id)
        {
            var roadmap = await context.Roadmaps.FindAsync(id);
            if (roadmap == null)
            {
                return NotFound();
            }

            context.Roadmaps.Remove(roadmap);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoadmapExists(int id)
        {
            return context.Roadmaps.Any(e => e.Id == id);
        }
    }
}
