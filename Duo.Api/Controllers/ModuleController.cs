using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Duo.Api.DTO.Requests;
using Duo.Api.Models;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing modules in the system
    /// </summary>
    [Route("module")]
    public class ModuleController : BaseController
    {
        public ModuleController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the next available position for a module in a course
        /// </summary>
        /// <param name="courseId">The ID of the course (nullable)</param>
        /// <returns>The next available position number</returns>
        private async Task<int> getLastPositionAsync(int? courseId)
        {
            if (courseId == null)
            {
                return 1;
            }

            List<Duo.Api.Models.Module> modules = await repository.GetModulesFromDbAsync();
            List<int> positions = modules
                        .Where(module => module.CourseId == courseId)
                        .OrderBy(module => module.Position)
                        .Select(module => module.Position)
                        .ToList();

            if (positions.Count == 0)
            {
                return 1;
            }

            for (int i = 1; i <= positions.Count; i++)
            {
                if (positions[i - 1] != i)
                {
                    return i;
                }
            }

            return positions.Count + 1;
        }

        /// <summary>
        /// Adds a new module to the system
        /// </summary>
        /// <param name="request">The module data to add</param>
        /// <returns>ActionResult with operation result</returns>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> addModule([FromForm] AddModuleRequest request)
        {
            try
            {
                Duo.Api.Models.Module module = new Duo.Api.Models.Module
                {
                    Title = request.Title,
                    Description = request.Description,
                    IsBonus = request.IsBonus,
                    Cost = request.Cost,
                    ImageUrl = request.ImageUrl,
                    Position = await this.getLastPositionAsync(request.CourseId),
                    CourseId = request.CourseId.HasValue ? request.CourseId.Value : null
                };


                await repository.AddModuleAsync(module);

                return Ok(new { message = "Module added successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Removes a module from the system
        /// </summary>
        /// <param name="id">The ID of the module to remove</param>
        /// <returns>ActionResult with operation result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> removeModule(int id)
        {
            try
            {
                var module = await repository.GetModuleByIdAsync(id);
                if (module == null)
                {
                    return NotFound(new { message = "Module not found!" });
                }

                await repository.DeleteModuleAsync(id);
                return Ok(new { message = "Module removed successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a module by its ID
        /// </summary>
        /// <param name="id">The ID of the module to retrieve</param>
        /// <returns>ActionResult with the module data or error message</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> getById(int id)
        {

            try
            {
                var module = await repository.GetModuleByIdAsync(id);
                if (module == null)
                {
                    return NotFound(new { message = "Module not found!" });
                }

                return Ok(new { result = module, message = "Successully got module!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a list of all modules in the system
        /// </summary>
        /// <returns>ActionResult with list of modules or error message</returns>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> listModules()
        {
            try
            {
                var modules = await repository.GetModulesFromDbAsync();
                return Ok(new { result = modules, message = "Successfully got list of modules" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a list of modules belonging to a specific course
        /// </summary>
        /// <param name="id">The ID of the course</param>
        /// <returns>ActionResult with list of modules or error message</returns>
        [HttpGet("list/course/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> listModulesByCourseId(int id)
        {
            try
            {
                List<Duo.Api.Models.Module> modules = await repository.GetModulesFromDbAsync();
                modules = modules
                            .Where(module => module.CourseId == id)
                            .ToList();
                if (!modules.Any())
                {
                    return NotFound(new { result = new List<Duo.Api.Models.Module>(), message = "No modules found for the specified course!" });
                }
                return Ok(new { result = modules, message = "Successfully got list of modules" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing module
        /// </summary>
        /// <param name="id">The ID of the module to update</param>
        /// <param name="request">The updated module data</param>
        /// <returns>ActionResult with operation result</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> updateModule(int id, [FromForm] UpdateModuleRequest request)
        {
            try
            {
                var module = await repository.GetModuleByIdAsync(id);
                if (module == null)
                {
                    return NotFound(new { message = "Module not found!" });
                }

                module.Title = request.Title ?? module.Title;
                module.Description = request.Description ?? module.Description;
                module.IsBonus = request.IsBonus ?? module.IsBonus;
                module.Cost = request.Cost ?? module.Cost;
                module.ImageUrl = request.ImageUrl ?? module.ImageUrl;
                module.CourseId = request.CourseId ?? module.CourseId;

                await repository.UpdateModuleAsync(module);
                return Ok(new { message = "Module updated successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}