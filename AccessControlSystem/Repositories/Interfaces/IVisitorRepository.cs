using AccessControlSystem.Entities;

namespace AccessControlSystem.Repositories.Interfaces;

public interface IVisitorRepository
{
    Task<Visitor?> GetByIdAsync(int id);
    Task<Visitor> AddAsync(Visitor visitor);
    Task UpdateAsync(Visitor visitor);
    Task<List<Visitor>> GetActiveVisitorsAsync();
}