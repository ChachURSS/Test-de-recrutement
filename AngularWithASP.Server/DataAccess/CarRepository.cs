using AngularWithASP.Server.Data;
using AngularWithASP.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularWithASP.Server.DataAccess
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;

        public CarRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Car?> GetCarById(int id)
        {
            return await _context.Cars.Include(c => c.Garage).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Car>> GetCars(int? garageId, string? brand, string? model, string? color, int page, int pageSize)
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

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCarsCount(int? garageId, string? brand, string? model, string? color)
        {
            var query = _context.Cars.AsQueryable();

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

            return await query.CountAsync();
        }

        public async Task<Car> AddCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return false;
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Car> UpdateCar(int id, Car car)
        {
            var existingCar = await _context.Cars.FindAsync(id);
            if (existingCar == null)
            {
                throw new KeyNotFoundException("Car not found");
            }

            existingCar.Brand = car.Brand;
            existingCar.Model = car.Model;
            existingCar.Color = car.Color;
            existingCar.GarageId = car.GarageId;

            _context.Entry(existingCar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingCar;
        }

        public async Task<Car> AssignCarToGarage(int carId, int garageId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                throw new KeyNotFoundException("Car not found");
            }

            var garage = await _context.Garages.FindAsync(garageId);
            if (garage == null)
            {
                throw new KeyNotFoundException("Garage not found");
            }

            car.GarageId = garageId;
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<Car> RemoveCarFromGarage(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                throw new KeyNotFoundException("Car not found");
            }

            car.GarageId = null;
            await _context.SaveChangesAsync();
            return car;
        }
    }
}
