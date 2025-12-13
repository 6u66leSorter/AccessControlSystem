using AccessControlSystem.Data;
using AccessControlSystem.Entities;
using AccessControlSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
        
    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }
        
    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees.FindAsync(id);
    }
        
    public async Task<Employee?> GetByCardNumberAsync(string cardNumber)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.CardNumber == cardNumber);
    }
        
    public async Task<List<Employee>> GetActiveEmployeesAsync()
    {
        return await _context.Employees
            .Where(e => e.IsActive)
            .OrderBy(e => e.FullName)
            .ToListAsync();
    }
        
    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
        
    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
}