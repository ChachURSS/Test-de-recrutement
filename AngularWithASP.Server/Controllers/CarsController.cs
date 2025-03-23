using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace AngularWithASP.Server.Controllers
{
    /// <summary>
    /// Represents a controller for managing cars.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CarsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarsController"/> class.
        /// </summary>
        /// <param name="context">Data context</param>
        /// <param name="logger">Logger object</param>
        public CarsController(AppDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the list of all cars, optionally filtered by garage ID, car brand car model, and car color.
        /// </summary>
        /// <param name="garageId">The ID of the garage to filter cars by.</param>
        /// <param name="brand">The brand to filter cars by.</param>
        /// <param name="model">The model to filter cars by.</param>
        /// <param name="color">The color to filter cars by.</param>
        /// <returns>A list of cars with their garage information.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieves the list of all cars, optionally filtered by garage ID, brand, model, and color.",
            Description = "Returns a list of cars with their garage information."
        )]
        [SwaggerResponse(200, "The list of cars.", typeof(IEnumerable<Car>))]
        [SwaggerResponse(204, "No cars were found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(
            [FromQuery] int? garageId,
            [FromQuery] string? brand,
            [FromQuery] string? model, 
            [FromQuery] string? color)
        {
            try
            {
                var query = _context.Cars.Include(c => c.Garage).AsQueryable();

                if (garageId.HasValue)
                {
                    query = query.Where(c => c.GarageId == garageId.Value);
                }

                if (!string.IsNullOrEmpty(brand))
                {
                    query = query.Where(c => c.Brand.Contains(brand));
                }

                if (!string.IsNullOrEmpty(model))
                {
                    query = query.Where(c => c.Model.Contains(model));
                }

                if (!string.IsNullOrEmpty(color))
                {
                    query = query.Where(c => c.Color.Contains(color));
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
        /// Adds a new car to the database.
        /// </summary>
        /// <param name="car">The car to add.</param>
        /// <returns>The created car.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Adds a new car to the database.",
            Description = "Returns the created car."
        )]
        [SwaggerResponse(201, "The created car.", typeof(Car))]
        [SwaggerResponse(400, "Brand and model are required.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(car.Brand) || string.IsNullOrEmpty(car.Model))
            {
                return BadRequest("Brand and model are required.");
            }

            try
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCars), new { id = car.Id }, car);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
        }

        /// <summary>
        /// Deletes a car by its ID.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>No content if the car was deleted, or NotFound if the car was not found.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes a car by its ID.",
            Description = "Returns NoContent if the car was deleted, or NotFound if the car was not found."
        )]
        [SwaggerResponse(204, "The car was deleted.")]
        [SwaggerResponse(404, "The car was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                {
                    return NotFound();
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
            
        }

        /// <summary>
        /// Assigns a car to a garage.
        /// </summary>
        /// <param name="carId">The ID of the car to assign.</param>
        /// <param name="garageId">The ID of the garage to assign the car to.</param>
        /// <returns>The updated car.</returns>
        [HttpPut("{carId}/assign/{garageId}")]
        [SwaggerOperation(
            Summary = "Assigns a car to a garage.",
            Description = "Returns the updated car."
        )]
        [SwaggerResponse(200, "The updated car.", typeof(Car))]
        [SwaggerResponse(404, "The car or garage was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<IActionResult> AssignCarToGarage(int carId, int garageId)
        {
            try
            {
                var car = await _context.Cars.FindAsync(carId);
                if (car == null)
                {
                    return NotFound($"Car {carId} not found");
                }

                var garage = await _context.Garages.FindAsync(garageId);
                if (garage == null)
                {
                    return NotFound($"Garage {garageId} not found");
                }

                car.GarageId = garageId;
                await _context.SaveChangesAsync();
                return Ok(car);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
            
        }

        /// <summary>
        /// Removes a car from its garage.
        /// </summary>
        /// <param name="carId">The ID of the car to remove from the garage.</param>
        /// <returns>The updated car.</returns>
        [HttpPut("{carId}/remove")]
        [SwaggerOperation(
            Summary = "Removes a car from its garage.",
            Description = "Returns the updated car."
        )]
        [SwaggerResponse(200, "The updated car.", typeof(Car))]
        [SwaggerResponse(404, "The car was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<IActionResult> RemoveCarFromGarage(int carId)
        {
            try
            {
                var car = await _context.Cars.FindAsync(carId);
                if (car == null)
                {
                    return NotFound();
                }

                car.GarageId = null;
                await _context.SaveChangesAsync();
                return Ok(car);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occured, please inform administrator");
            }
        }
    }
}
