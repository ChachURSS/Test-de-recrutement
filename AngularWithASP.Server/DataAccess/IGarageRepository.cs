using AngularWithASP.Server.Models;

namespace AngularWithASP.Server.DataAccess
{
    public interface IGarageRepository
    {
        Task<Garage> AddGarage(Garage garage);
        Task<bool> DeleteGarage(int id);
        Task<Garage?> GetGarageById(int id);
        Task<IEnumerable<Garage>> GetGarages(string? name);
        Task<Garage> UpdateGarage(int id, Garage garage);
    }
}