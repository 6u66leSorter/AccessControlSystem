using AccessControlSystem.Dtos;
using AccessControlSystem.Entities;
using AccessControlSystem.Services;
using AccessControlSystem.Services.Interfaces;

namespace AccessControlSystem.ConsoleApp.UI.Handlers
{
    public class EmployeeHandler
    {
        private readonly IAccessControlService _service;
        
        public EmployeeHandler(IAccessControlService service)
        {
            _service = service;
        }
        
        public async Task CheckEmployeeAccess()
        {
            Console.Clear();
            Console.WriteLine("=== ПРОВЕРКА ДОСТУПА СОТРУДНИКА ===");
            
            Console.Write("Номер карты: ");
            var cardNumber = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                Console.WriteLine("❌ Все поля обязательны!");
                return;
            }
            
            var result = await _service.ValidateEmployeeAccessAsync(cardNumber);
            DisplayResult(result);
        }

        public async Task CreateEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== СОЗДАНИЕ НОВОГО СОТРУДНИКА ===\n");

            // ФИО
            Console.Write("ФИО сотрудника: ");
            var fullName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.WriteLine("❌ ФИО обязательно!");
                return;
            }

            // Номер карты
            Console.Write("Номер карты доступа: ");
            var cardNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                Console.WriteLine("❌ Номер карты обязателен!");
                return;
            }

            // Статус активности
            Console.Write("Активный сотрудник? (y/n): ");
            var isActiveInput = Console.ReadLine()?.ToLower();
            var isActive = isActiveInput is not ("n" or "no" or "false");

            // Подтверждение
            Console.WriteLine("\n=== ПОДТВЕРЖДЕНИЕ ДАННЫХ ===");
            Console.WriteLine($"ФИО: {fullName}");
            Console.WriteLine($"Номер карты: {cardNumber}");
            Console.WriteLine($"Статус: {(isActive ? "Активен" : "Неактивен")}");

            Console.Write("\nСоздать сотрудника? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();

            if (confirm != "y")
            {
                Console.WriteLine("❌ Создание отменено");
                return;
            }

            try
            {
                var employee = new Employee
                {
                    FullName = fullName,
                    CardNumber = cardNumber,
                    IsActive = isActive,
                    CreatedAt = DateTime.UtcNow
                };

                var createdEmployee = await _service.CreateEmployeeAsync(employee);

                if (createdEmployee != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n✅ Сотрудник успешно создан!");
                    Console.ResetColor();
                    Console.WriteLine($"ID: {createdEmployee.Id}");
                    Console.WriteLine($"ФИО: {createdEmployee.FullName}");
                    Console.WriteLine($"Карта: {createdEmployee.CardNumber}");
                    Console.WriteLine($"Статус: {(createdEmployee.IsActive ? "Активен" : "Неактивен")}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Ошибка при создании сотрудника!");
                    Console.ResetColor();
                    Console.WriteLine("Возможно, сотрудник с таким номером карты уже существует.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void DisplayResult(AccessResult result)
        {
            Console.WriteLine("\n" + new string('=', 40));
            if (result.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ ДОСТУП РАЗРЕШЕН");
                Console.ResetColor();
                Console.WriteLine($"Сотрудник: {result.EmployeeName}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ ДОСТУП ЗАПРЕЩЕН");
                Console.ResetColor();
                Console.WriteLine($"Причина: {result.Reason}");
                if (!string.IsNullOrEmpty(result.EmployeeName))
                    Console.WriteLine($"Сотрудник: {result.EmployeeName}");
            }
            Console.WriteLine($"Время: {result.Timestamp:HH:mm:ss}");
            Console.WriteLine(new string('=', 40));
        }
    }
}