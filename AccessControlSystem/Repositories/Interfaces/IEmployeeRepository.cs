using AccessControlSystem.Entities;

namespace AccessControlSystem.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByCardNumberAsync(string cardNumber);
    Task<List<Employee>> GetActiveEmployeesAsync();
    Task<Employee> AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
}