using AccessControlSystem.Dtos;
using AccessControlSystem.Entities;

namespace AccessControlSystem.Services.Interfaces;
    
public interface IAccessControlService
{
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<AccessResult> ValidateEmployeeAccessAsync(string cardNumber);
    Task<VehiclePass> RegisterVehicleEntryAsync(string vehicleNumber, string driverName, 
        string organization, bool isInternal);
    Task<bool> MarkVehicleExitAsync(int vehicleId);
    Task<Visitor> RegisterVisitorAsync(string fullName, string organization, 
        string purpose, string contactPerson);
    Task<bool> MarkVisitorExitAsync(int visitorId);
    Task<List<VehiclePass>> GetActiveVehiclesAsync();
    Task<List<AccessLog>> GetTodayAccessLogsAsync();
}