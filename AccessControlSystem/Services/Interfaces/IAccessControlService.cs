using AccessControlSystem.Dtos;
using AccessControlSystem.Entities;

namespace AccessControlSystem.Services.Interfaces;
    
public interface IAccessControlService
{
    // Для сотрудников
    Task<AccessResult> ValidateEmployeeAccessAsync(string cardNumber, string checkpoint);
        
    // Для транспорта
    Task<VehiclePass> RegisterVehicleEntryAsync(string vehicleNumber, string driverName, 
        string organization, bool isInternal, string checkpoint);
    Task<bool> MarkVehicleExitAsync(int vehicleId, string checkpoint);
        
    // Для посетителей
    Task<Visitor> RegisterVisitorAsync(string fullName, string organization, 
        string purpose, string contactPerson, string checkpoint);
    Task<bool> MarkVisitorExitAsync(int visitorId, string checkpoint);
        
    // Получение данных
    Task<List<VehiclePass>> GetActiveVehiclesAsync();
    Task<List<Visitor>> GetActiveVisitorsAsync();
    Task<List<AccessLog>> GetTodayAccessLogsAsync();
}