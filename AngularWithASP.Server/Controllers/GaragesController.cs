using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;
using Swashbuckle.AspNetCore.Annotations;
using AngularWithASP.Server.DataAccess;

namespace AngularWithASP.Server.Controllers
{
    /// <summary>
    /// Represents a controller for managing garages.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GaragesController : ControllerBase
    {
        private readonly IGarageRepository _garageRepository;
        private readonly ILogger<GaragesController> _logger;

        /// <summary>
        /// Constructor for the <see cref="GaragesController"/> class.
        /// </summary>
        /// <param name="garageRepository">Garage repository</param>
        /// <param name="logger">Logger object</param>
        public GaragesController(IGarageRepository garageRepository, ILogger<GaragesController> logger)
        {
            _garageRepository = garageRepository;
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
                var garages = await _garageRepository.GetGarages(name);

                if (!garages.Any())
                {
                    return NoContent();
                }

                return Ok(garages);
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

                var createdGarage = await _garageRepository.AddGarage(garage);
                return CreatedAtAction(nameof(GetGarages), new { id = createdGarage.Id }, createdGarage);
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
                var deleted = await _garageRepository.DeleteGarage(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
        }

        /// <summary>
        /// Updates a garage based on its ID.
        /// </summary>
        /// <param name="id">The ID of the garage to update.</param>
        /// <param name="garage">The garage data to update.</param>
        /// <returns>The updated garage.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates a garage based on its ID.",
            Description = "Returns the updated garage."
        )]
        [SwaggerResponse(200, "The updated garage.", typeof(Garage))]
        [SwaggerResponse(400, "Invalid garage data.")]
        [SwaggerResponse(404, "The garage was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<IActionResult> UpdateGarage(int id, [FromBody] Garage garage)
        {
            if (id != garage.Id)
            {
                return BadRequest("Garage ID mismatch.");
            }

            if (garage == null)
            {
                return BadRequest("Invalid garage data.");
            }

            if (string.IsNullOrEmpty(garage.Name))
            {
                return BadRequest("Name is required.");
            }

            try
            {
                var updatedGarage = await _garageRepository.UpdateGarage(id, garage);
                return Ok(updatedGarage);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
            }
        }
        /// <summary>
        /// Retrieves a garage by its ID.
        /// </summary>
        /// <param name="id">The ID of the garage to retrieve.</param>
        /// <returns>The garage with the specified ID.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Retrieves a garage by its ID.",
            Description = "Returns the garage with the specified ID."
        )]
        [SwaggerResponse(200, "The garage with the specified ID.", typeof(Garage))]
        [SwaggerResponse(404, "The garage was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<ActionResult<Garage>> GetGarageById(
            [SwaggerParameter("The ID of the garage to retrieve.")] int id)
        {
            try
            {
                var garage = await _garageRepository.GetGarageById(id);

                if (garage == null)
                {
                    return NotFound();
                }

                return Ok(garage);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
            }
        }
    }
}
