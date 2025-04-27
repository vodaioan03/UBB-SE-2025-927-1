using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Persistence;

namespace Duo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadmapsController : ControllerBase
    {
        private readonly DataContext _context;

        public RoadmapsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Roadmaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roadmap>>> GetRoadmap()
        {
            return await _context.Roadmaps.ToListAsync();
        }

        // GET: api/Roadmaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Roadmap>> GetRoadmap(int id)
        {
            var roadmap = await _context.Roadmaps.FindAsync(id);

            if (roadmap == null)
            {
                return NotFound();
            }

            return roadmap;
        }

        // PUT: api/Roadmaps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoadmap(int id, Roadmap roadmap)
        {
            if (id != roadmap.Id)
            {
                return BadRequest();
            }

            _context.Entry(roadmap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        [HttpPost]
        public async Task<ActionResult<Roadmap>> PostRoadmap(Roadmap roadmap)
        {
            _context.Roadmaps.Add(roadmap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoadmap", new { id = roadmap.Id }, roadmap);
        }

        // DELETE: api/Roadmaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoadmap(int id)
        {
            var roadmap = await _context.Roadmaps.FindAsync(id);
            if (roadmap == null)
            {
                return NotFound();
            }

            _context.Roadmaps.Remove(roadmap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoadmapExists(int id)
        {
            return _context.Roadmaps.Any(e => e.Id == id);
        }
    }
}
