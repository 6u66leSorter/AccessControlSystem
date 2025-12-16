using AccessControlSystem.Services;
using AccessControlSystem.Services.Interfaces;

namespace AccessControlSystem.ConsoleApp.UI.Handlers;

public class VehicleHandler
    {
        private readonly IAccessControlService _service;
        
        public VehicleHandler(IAccessControlService service)
        {
            _service = service;
        }
        
        public async Task RegisterVehicleEntry()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ ВЪЕЗДА ТРАНСПОРТА ===");
            
            Console.Write("Государственный номер: ");
            var vehicleNumber = Console.ReadLine();
            
            Console.Write("ФИО водителя: ");
            var driverName = Console.ReadLine();
            
            Console.Write("Организация: ");
            var organization = Console.ReadLine();
            
            Console.Write("Свой транспорт? (да/нет): ");
            var isInternal = Console.ReadLine()?.ToLower() == "да";
            
            if (string.IsNullOrWhiteSpace(vehicleNumber) || string.IsNullOrWhiteSpace(driverName) || 
                string.IsNullOrWhiteSpace(organization))
            {
                Console.WriteLine("❌ Все поля обязательны!");
                return;
            }
            
            var vehicle = await _service.RegisterVehicleEntryAsync(
                vehicleNumber, driverName, organization, isInternal);
            
            Console.WriteLine($"\n✅ Транспорт зарегистрирован!");
            Console.WriteLine($"ID пропуска: {vehicle.Id}");
            Console.WriteLine($"Время въезда: {vehicle.EntryTime:HH:mm:ss}");
        }
        
        public async Task MarkVehicleExit()
        {
            Console.Clear();
            Console.WriteLine("=== ОТМЕТКА ВЫЕЗДА ТРАНСПОРТА ===");
            
            Console.Write("ID пропуска: ");
            if (!int.TryParse(Console.ReadLine(), out int passId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }
            
            var success = await _service.MarkVehicleExitAsync(passId);
            
            if (success)
                Console.WriteLine("\n✅ Выезд отмечен!");
            else
                Console.WriteLine("\n❌ Пропуск не найден или уже закрыт!");
        }
        
        public async Task ShowActiveVehicles()
        {
            Console.Clear();
            Console.WriteLine("=== АКТИВНЫЙ ТРАНСПОРТ НА ТЕРРИТОРИИ ===\n");
            
            var vehicles = await _service.GetActiveVehiclesAsync();
            
            if (!vehicles.Any())
            {
                Console.WriteLine("На территории нет активного транспорта");
                return;
            }
            
            ConsoleHelpers.PrintVehiclesTable(vehicles);
        }
        
    }