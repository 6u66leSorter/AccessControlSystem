using Microsoft.Extensions.DependencyInjection;
using AccessControlSystem.Services;
using AccessControlSystem.ConsoleApp.UI.Handlers;
using AccessControlSystem.Services.Interfaces;

namespace AccessControlSystem.ConsoleApp.UI;

public class MenuService
    {
        private readonly IAccessControlService _accessService;
        private readonly EmployeeHandler _employeeHandler;
        private readonly VehicleHandler _vehicleHandler;
        private readonly VisitorHandler _visitorHandler;
        private readonly LogHandler _logHandler;
        
        public MenuService(
            IAccessControlService accessService,
            EmployeeHandler employeeHandler,
            VehicleHandler vehicleHandler,
            VisitorHandler visitorHandler,
            LogHandler logHandler)
        {
            _accessService = accessService;
            _employeeHandler = employeeHandler;
            _vehicleHandler = vehicleHandler;
            _visitorHandler = visitorHandler;
            _logHandler = logHandler;
        }
        
        public async Task RunAsync()
        {
            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();
                
                try
                {
                    var shouldExit = await ProcessChoiceAsync(choice);
                    if (shouldExit) break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Ошибка: {ex.Message}");
                }
                
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
        
        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("     АРМ ОХРАННИКА КПП");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Зарегистрировать сотрудника");
            Console.WriteLine("2. Проверить доступ сотрудника");
            Console.WriteLine("3. Зарегистрировать въезд транспорта");
            Console.WriteLine("4. Отметить выезд транспорта");
            Console.WriteLine("5. Зарегистрировать посетителя");
            Console.WriteLine("6. Отметить выход посетителя");
            Console.WriteLine("7. Активный транспорт на территории");
            //Console.WriteLine("8. Активные посетители");
            Console.WriteLine("9. Журнал событий за сегодня");
            Console.WriteLine("0. Выход");
            Console.WriteLine("====================================");
            Console.Write("Выберите действие: ");
        }
        
        private async Task<bool> ProcessChoiceAsync(string choice)
        {
            return choice switch
            {
                "1" => await ExecuteAndReturnFalse(async () => await _employeeHandler.CreateEmployee()),
                "2" => await ExecuteAndReturnFalse(async () => await _employeeHandler.CheckEmployeeAccess()),
                "3" => await ExecuteAndReturnFalse(async () => await _vehicleHandler.RegisterVehicleEntry()),
                "4" => await ExecuteAndReturnFalse(async () => await _vehicleHandler.MarkVehicleExit()),
                "5" => await ExecuteAndReturnFalse(async () => await _visitorHandler.RegisterVisitor()),
                "6" => await ExecuteAndReturnFalse(async () => await _visitorHandler.MarkVisitorExit()),
                "7" => await ExecuteAndReturnFalse(async () => await _vehicleHandler.ShowActiveVehicles()),
                //"8" => await ExecuteAndReturnFalse(async () => await _visitorHandler.ShowActiveVisitors()),
                "9" => await ExecuteAndReturnFalse(async () => await _logHandler.ShowTodayLogs()),
                "0" => await ExecuteAndReturnTrue(() =>
                {
                    Console.WriteLine("Выход из системы...");
                    return Task.CompletedTask;
                }),
                _ => await ExecuteAndReturnFalse(() =>
                {
                    Console.WriteLine("Неверный выбор!");
                    return Task.CompletedTask;
                })
            };
        }
        
        private async Task<bool> ExecuteAndReturnFalse(Func<Task> action)
        {
            await action();
            return false;
        }

        private async Task<bool> ExecuteAndReturnTrue(Func<Task> action)
        {
            await action();
            return true;
        }
    }