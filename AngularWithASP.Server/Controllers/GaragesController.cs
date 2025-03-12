using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;

namespace AngularWithASP.Server.Controllers
{
    [Route("api/garages")]
    [ApiController]
    public class GaragesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GaragesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Garage>>> GetGarages()
        {
            return await _context.Garages.Include(g => g.Cars).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Garage>> AddGarage(Garage garage)
        {
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGarages), new { id = garage.Id }, garage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarage(int id)
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
    }
}
