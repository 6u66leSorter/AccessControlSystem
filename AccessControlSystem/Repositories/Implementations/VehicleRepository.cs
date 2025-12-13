using AccessControlSystem.Data;
using AccessControlSystem.Entities;
using AccessControlSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Repositories.Implementations;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;
        
    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }
        
    public async Task<VehiclePass?> GetByIdAsync(int id)
    {
        return await _context.VehiclePasses.FindAsync(id);
    }
        
    public async Task<VehiclePass?> GetByVehicleNumberAsync(string vehicleNumber)
    {
        return await _context.VehiclePasses
            .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleNumber);
    }
        
    public async Task<List<VehiclePass>> GetActiveVehiclesAsync()
    {
        return await _context.VehiclePasses
            .Where(v => !v.ExitTime.HasValue)
            .OrderBy(v => v.EntryTime)
            .ToListAsync();
    }
        
    public async Task<VehiclePass> AddAsync(VehiclePass vehicle)
    {
        await _context.VehiclePasses.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }
        
    public async Task UpdateAsync(VehiclePass vehicle)
    {
        _context.VehiclePasses.Update(vehicle);
        await _context.SaveChangesAsync();
    }
}