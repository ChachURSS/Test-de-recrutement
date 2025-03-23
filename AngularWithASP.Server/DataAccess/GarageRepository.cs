using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWithASP.Server.DataAccess
{
    public class GarageRepository : IGarageRepository
    {
        private readonly AppDbContext _context;

        public GarageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Garage>> GetGarages(string? name)
        {
            var query = _context.Garages.Include(g => g.Cars).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(g => g.Name.Contains(name));
            }

            return await query.ToListAsync();
        }

        public async Task<Garage?> GetGarageById(int id)
        {
            return await _context.Garages.Include(g => g.Cars).FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Garage> AddGarage(Garage garage)
        {
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            return garage;
        }

        public async Task<bool> DeleteGarage(int id)
        {
            var garage = await _context.Garages.FindAsync(id);
            if (garage == null)
            {
                return false;
            }

            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Garage> UpdateGarage(int id, Garage garage)
        {
            var existingGarage = await _context.Garages.FindAsync(id);
            if (existingGarage == null)
            {
                throw new KeyNotFoundException("Garage not found");
            }

            existingGarage.Name = garage.Name;
            existingGarage.Address = garage.Address;
            existingGarage.City = garage.City;

            _context.Entry(existingGarage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingGarage;
        }

    }
}
