using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;

namespace AngularWithASP.Server.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.Include(c => c.Garage).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCars), new { id = car.Id }, car);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
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

        [HttpPut("{carId}/assign/{garageId}")]
        public async Task<IActionResult> AssignCarToGarage(int carId, int garageId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return NotFound();
            }

            car.GarageId = garageId;
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [HttpPut("{carId}/remove")]
        public async Task<IActionResult> RemoveCarFromGarage(int carId)
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
    }
}
