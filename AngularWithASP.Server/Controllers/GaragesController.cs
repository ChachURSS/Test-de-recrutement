using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace AngularWithASP.Server.Controllers
{
    /// <summary>
    /// Represents a controller for managing garages.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GaragesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GaragesController> _logger;

        /// <summary>
        /// Constructor for the <see cref="GaragesController"/> class.
        /// </summary>
        /// <param name="context">Data context</param>
        /// <param name="logger">Logger object</param>
        public GaragesController(AppDbContext context, ILogger<GaragesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the list of all garages, optionally filtered by name.
        /// </summary>
        /// <param name="name">The name to filter garages by.</param>
        /// <returns>A list of garages with their cars information.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieves the list of all garages, optionally filtered by name.",
            Description = "Returns a list of garages with their cars information."
        )]
        [SwaggerResponse(200, "The list of garages.", typeof(IEnumerable<Garage>))]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        [SwaggerResponse(204, "No garages were found.")]
        public async Task<ActionResult<IEnumerable<Garage>>> GetGarages(
             [SwaggerParameter("The name to filter garages by.")][FromQuery] string? name)
        {
            try
            {
                var query = _context.Garages.Include(g => g.Cars).AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(g => g.Name.Contains(name));
                }

                if (query.Count() == 0)
                {
                    return NoContent();
                }

                return await query.ToListAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
        }

        /// <summary>
        /// Adds a new garage to the database.
        /// </summary>
        /// <param name="garage">The garage to add.</param>
        /// <returns>The created garage.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Adds a new garage to the database.",
            Description = "Returns the created garage."
        )]
        [SwaggerResponse(201, "The created garage.", typeof(Garage))]
        [SwaggerResponse(400, "The garage is null or invalid.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<ActionResult<Garage>> AddGarage(
            [SwaggerParameter("Attribute for garage being created")] Garage garage)
        {
            try
            {
                if (garage == null)
                {
                    return BadRequest();
                }

                if (string.IsNullOrEmpty(garage.Name))
                {
                    return BadRequest("Name is required.");
                }

                _context.Garages.Add(garage);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGarages), new { id = garage.Id }, garage);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
        }

        /// <summary>
        /// Deletes a garage by its ID.
        /// </summary>
        /// <param name="id">The ID of the garage to delete.</param>
        /// <returns>No content if the garage was deleted, or NotFound if the garage was not found.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes a garage by its ID.",
            Description = "Returns no content if the garage was deleted, or not found if the garage was not found. Cars in the carage will not be deleted but linked to nothing"
        )]
        [SwaggerResponse(204, "The garage was deleted.")]
        [SwaggerResponse(404, "The garage was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<IActionResult> DeleteGarage(
            [SwaggerParameter("Id of the garage to delete")] int id)
        {
            try
            {
                var garage = await _context.Garages.FindAsync(id);
                if (garage == null)
                {
                    return NotFound();
                }

                _context.Garages.Remove(garage);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
        }
    }
}
