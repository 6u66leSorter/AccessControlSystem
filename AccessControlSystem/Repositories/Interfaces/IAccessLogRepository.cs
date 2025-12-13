using AccessControlSystem.Entities;

namespace AccessControlSystem.Repositories.Interfaces;

public interface IAccessLogRepository
{
    Task<List<AccessLog>> GetTodayLogsAsync();
    Task<AccessLog> AddAsync(AccessLog log);
}
