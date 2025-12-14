using AccessControlSystem.Dtos;
using AccessControlSystem.Entities;
using AccessControlSystem.Repositories.Interfaces;
using AccessControlSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Services.Implementations;

public class AccessControlService : IAccessControlService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVisitorRepository _visitorRepository;
        private readonly IAccessLogRepository _accessLogRepository;
        private readonly ILogger<AccessControlService> _logger;
        
        public AccessControlService(
            IEmployeeRepository employeeRepository,
            IVehicleRepository vehicleRepository,
            IVisitorRepository visitorRepository,
            IAccessLogRepository accessLogRepository,
            ILogger<AccessControlService> logger)
        {
            _employeeRepository = employeeRepository;
            _vehicleRepository = vehicleRepository;
            _visitorRepository = visitorRepository;
            _accessLogRepository = accessLogRepository;
            _logger = logger;
        }
        
        public async Task<AccessResult> ValidateEmployeeAccessAsync(string cardNumber, string checkpoint)
        {
            _logger.LogInformation("Проверка доступа по карте: {CardNumber}", cardNumber);
            
            var employee = await _employeeRepository.GetByCardNumberAsync(cardNumber);
            
            if (employee == null)
            {
                await LogAccessAsync(EntityType.Employee, 0, false, checkpoint, "Сотрудник не найден");
                return AccessResult.Denied("Сотрудник не найден");
            }
            
            if (!employee.IsActive)
            {
                await LogAccessAsync(EntityType.Employee, employee.Id, false, checkpoint, "Сотрудник неактивен");
                return AccessResult.Denied("Сотрудник неактивен", employee.FullName);
            }
            
            await LogAccessAsync(EntityType.Employee, employee.Id, true, checkpoint, null);
            return AccessResult.Granted(employee.FullName);
        }
        
        public async Task<VehiclePass> RegisterVehicleEntryAsync(
            string vehicleNumber, 
            string driverName, 
            string organization, 
            bool isInternal, 
            string checkpoint)
        {
            _logger.LogInformation("Регистрация въезда транспорта: {VehicleNumber}", vehicleNumber);
            
            var activeVehicle = await _vehicleRepository.GetByVehicleNumberAsync(vehicleNumber);
            if (activeVehicle != null && !activeVehicle.IsCompleted)
            {
                throw new InvalidOperationException($"Транспорт {vehicleNumber} уже на территории");
            }
            
            var vehicle = new VehiclePass
            {
                VehicleNumber = vehicleNumber,
                DriverName = driverName,
                Organization = organization,
                IsInternal = isInternal,
                EntryTime = DateTime.Now
            };
            
            var createdVehicle = await _vehicleRepository.AddAsync(vehicle);
            
            await LogAccessAsync(EntityType.Vehicle, createdVehicle.Id, true, checkpoint, "Въезд транспорта");
            
            return createdVehicle;
        }
        
        public async Task<bool> MarkVehicleExitAsync(int vehicleId, string checkpoint)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null || vehicle.IsCompleted)
                return false;
            
            vehicle.ExitTime = DateTime.Now;
            await _vehicleRepository.UpdateAsync(vehicle);
            
            await LogAccessAsync(EntityType.Vehicle, vehicle.Id, false, checkpoint, "Выезд транспорта");
            
            return true;
        }
        
        public async Task<Visitor> RegisterVisitorAsync(
            string fullName, 
            string organization, 
            string purpose, 
            string contactPerson, 
            string checkpoint)
        {
            var visitor = new Visitor
            {
                FullName = fullName,
                Organization = organization,
                Purpose = purpose,
                ContactPerson = contactPerson,
                EntryTime = DateTime.Now
            };
            
            var createdVisitor = await _visitorRepository.AddAsync(visitor);
            
            await LogAccessAsync(EntityType.Visitor, createdVisitor.Id, true, checkpoint, "Вход посетителя");
            
            return createdVisitor;
        }
        
        public async Task<bool> MarkVisitorExitAsync(int visitorId, string checkpoint)
        {
            var visitor = await _visitorRepository.GetByIdAsync(visitorId);
            if (visitor == null || visitor.IsCompleted)
                return false;
            
            visitor.ExitTime = DateTime.Now;
            await _visitorRepository.UpdateAsync(visitor);
            
            await LogAccessAsync(EntityType.Visitor, visitor.Id, false, checkpoint, "Выход посетителя");
            
            return true;
        }
        
        public async Task<List<VehiclePass>> GetActiveVehiclesAsync()
        {
            return await _vehicleRepository.GetActiveVehiclesAsync();
        }
        
        public async Task<List<Visitor>> GetActiveVisitorsAsync()
        {
            return await _visitorRepository.GetActiveVisitorsAsync();
        }
        
        public async Task<List<AccessLog>> GetTodayAccessLogsAsync()
        {
            return await _accessLogRepository.GetTodayLogsAsync();
        }
        
        private async Task LogAccessAsync(
            EntityType entityType, 
            int entityId, 
            bool isEntry, 
            string checkpoint, 
            string? reason)
        {
            var log = new AccessLog
            {
                EntityType = entityType,
                EntityId = entityId,
                AccessTime = DateTime.Now,
                IsEntry = isEntry,
                CheckpointNumber = checkpoint,
                Reason = reason
            };
            
            switch (entityType)
            {
                case EntityType.Employee:
                    log.EmployeeId = entityId;
                    break;
                case EntityType.Vehicle:
                    log.VehiclePassId = entityId;
                    break;
                case EntityType.Visitor:
                    log.VisitorId = entityId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
            }
            
            await _accessLogRepository.AddAsync(log);
        }
    }