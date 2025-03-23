using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;
using Swashbuckle.AspNetCore.Annotations;
using AngularWithASP.Server.DataAccess;

namespace AngularWithASP.Server.Controllers
{
    /// <summary>
    /// Represents a controller for managing cars.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarsController"/> class.
        /// </summary>
        /// <param name="carRepository">Car repository</param>
        /// <param name="logger">Logger object</param>
        public CarsController(ICarRepository carRepository, ILogger<CarsController> logger)
        {
            _carRepository = carRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves information for a specific car by its ID.
        /// </summary>
        /// <param name="id">The ID of the car to retrieve.</param>
        /// <returns>The car information.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Retrieves information for a specific car by its ID.",
            Description = "Returns the car information."
        )]
        [SwaggerResponse(200, "The car information.", typeof(Car))]
        [SwaggerResponse(404, "The car was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<ActionResult<Car>> GetCarById(int id)
        {
            try
            {
                var car = await _carRepository.GetCarById(id);
                if (car == null)
                {
                    return NotFound();
                }

                return Ok(car);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
            }
        }

        /// <summary>
        /// Retrieves the list of all cars, optionally filtered by garage ID, car brand, car model, and car color, with pagination.
        /// </summary>
        /// <param name="garageId">The ID of the garage to filter cars by.</param>
        /// <param name="brand">The brand to filter cars by.</param>
        /// <param name="model">The model to filter cars by.</param>
        /// <param name="color">The color to filter cars by.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of cars with their garage information.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieves the list of all cars, optionally filtered by garage ID, brand, model, and color, with pagination.",
            Description = "Returns a list of cars with their garage information."
        )]
        [SwaggerResponse(200, "The list of cars.", typeof(IEnumerable<Car>))]
        [SwaggerResponse(204, "No cars were found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(
            [FromQuery] int? garageId,
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? color,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var cars = await _carRepository.GetCars(garageId, brand, model, color, page, pageSize);
                var totalItems = await _carRepository.GetTotalCarsCount(garageId, brand, model, color);

                if (!cars.Any())
                {
                    return NoContent();
                }

                Response.Headers.Add("X-Total-Count", totalItems.ToString());

                return Ok(cars);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
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
                var createdCar = await _carRepository.AddCar(car);
                return CreatedAtAction(nameof(GetCars), new { id = createdCar.Id }, createdCar);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
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
                var deleted = await _carRepository.DeleteCar(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
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
                var updatedCar = await _carRepository.AssignCarToGarage(carId, garageId);
                return Ok(updatedCar);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
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
                var updatedCar = await _carRepository.RemoveCarFromGarage(carId);
                return Ok(updatedCar);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.GetFullStack());
                return StatusCode(500, "An internal error occurred, please inform administrator");
            }
        }

        /// <summary>
        /// Updates a car based on its ID.
        /// </summary>
        /// <param name="id">The ID of the car to update.</param>
        /// <param name="car">The car data to update.</param>
        /// <returns>The updated car.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates a car based on its ID.",
            Description = "Returns the updated car."
        )]
        [SwaggerResponse(200, "The updated car.", typeof(Car))]
        [SwaggerResponse(400, "Invalid car data.")]
        [SwaggerResponse(404, "The car was not found.")]
        [SwaggerResponse(500, "An internal error occurred while processing the request.")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest("Car ID mismatch.");
            }

            if (car == null)
            {
                return BadRequest("Invalid car data.");
            }

            try
            {
                var updatedCar = await _carRepository.UpdateCar(id, car);
                return Ok(updatedCar);
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
    }
}
