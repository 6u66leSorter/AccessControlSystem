using AccessControlSystem.Data;
using AccessControlSystem.Entities;
using AccessControlSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Repositories.Implementations;

public class VisitorRepository : IVisitorRepository
{
    private readonly AppDbContext _context;
        
    public VisitorRepository(AppDbContext context)
    {
        _context = context;
    }
        
    public async Task<Visitor?> GetByIdAsync(int id)
    {
        return await _context.Visitors
            .FirstOrDefaultAsync(v => v.Id == id);
    }
        
    public async Task<Visitor> AddAsync(Visitor visitor)
    {
        await _context.Visitors.AddAsync(visitor);
        await _context.SaveChangesAsync();
        return visitor;
    }
        
    public async Task UpdateAsync(Visitor visitor)
    {
        _context.Visitors.Update(visitor);
        await _context.SaveChangesAsync();
    }
        
    public async Task<List<Visitor>> GetActiveVisitorsAsync()
    {
        return await _context.Visitors
            .Where(v => !v.ExitTime.HasValue)
            .OrderByDescending(v => v.EntryTime)
            .ToListAsync();
    }
}