using Microsoft.AspNetCore.Mvc;
using Duo.Api.Models;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using System.Threading.Tasks;
using System.Linq;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("tag")]
    public class TagController : BaseController
    {
        public TagController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Adds a new tag to the database.
        /// </summary>
        /// <param name="tag">The tag data to add.</param>
        /// <returns>The added tag.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddTag([FromForm] Tag tag)
        {
            await repository.AddTagAsync(tag);
            return Ok(tag);
        }

        /// <summary>
        /// Retrieves a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to retrieve.</param>
        /// <returns>The tag if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetTag([FromQuery] int id)
        {
            var tag = await repository.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }

        /// <summary>
        /// Lists all tags in the database.
        /// </summary>
        /// <returns>A list of all tags.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListTags()
        {
            var tags = await repository.GetTagsFromDbAsync();
            return Ok(tags);
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="updatedTag">The updated tag data, including TagId.</param>
        /// <returns>The updated tag if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTag([FromForm] Tag updatedTag)
        {
            var tag = await repository.GetTagByIdAsync(updatedTag.TagId);
            if (tag == null)
                return NotFound();
            await repository.UpdateTagAsync(updatedTag);
            return Ok(updatedTag);
        }

        /// <summary>
        /// Deletes a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTag([FromQuery] int id)
        {
            var tag = await repository.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound();
            await repository.DeleteTagAsync(id);
            return Ok();
        }
    }
}
