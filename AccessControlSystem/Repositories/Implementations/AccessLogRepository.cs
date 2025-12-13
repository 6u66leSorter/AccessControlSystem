using AccessControlSystem.Data;
using AccessControlSystem.Entities;
using AccessControlSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Repositories.Implementations;

public class AccessLogRepository : IAccessLogRepository
{
    private readonly AppDbContext _context;
        
    public AccessLogRepository(AppDbContext context)
    {
        _context = context;
    }
        
    public async Task<List<AccessLog>> GetTodayLogsAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
            
        return await _context.AccessLogs
            .Include(a => a.Employee)
            .Include(a => a.VehiclePass)
            .Include(a => a.Visitor)
            .Where(a => a.AccessTime >= today && a.AccessTime < tomorrow)
            .OrderByDescending(a => a.AccessTime)
            .Take(50)
            .ToListAsync();
    }
        
    public async Task<AccessLog> AddAsync(AccessLog log)
    {
        await _context.AccessLogs.AddAsync(log);
        await _context.SaveChangesAsync();
        return log;
    }
}