using AngularWithASP.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngularWithASP.Server.DataAccess
{
    public interface ICarRepository
    {
        Task<Car?> GetCarById(int id);
        Task<IEnumerable<Car>> GetCars(int? garageId, string? brand, string? model, string? color, int page, int pageSize);
        Task<int> GetTotalCarsCount(int? garageId, string? brand, string? model, string? color);
        Task<Car> AddCar(Car car);
        Task<bool> DeleteCar(int id);
        Task<Car> UpdateCar(int id, Car car);
        Task<Car> AssignCarToGarage(int carId, int garageId);
        Task<Car> RemoveCarFromGarage(int carId);
    }
}
