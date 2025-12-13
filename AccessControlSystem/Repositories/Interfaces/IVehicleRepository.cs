using AccessControlSystem.Entities;

namespace AccessControlSystem.Repositories.Interfaces;

public interface IVehicleRepository
{
    Task<VehiclePass?> GetByIdAsync(int id);
    Task<VehiclePass?> GetByVehicleNumberAsync(string vehicleNumber);
    Task<List<VehiclePass>> GetActiveVehiclesAsync();
    Task<VehiclePass> AddAsync(VehiclePass vehicle);
    Task UpdateAsync(VehiclePass vehicle);
}