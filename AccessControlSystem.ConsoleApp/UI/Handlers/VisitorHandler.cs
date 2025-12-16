using AccessControlSystem.Services;
using AccessControlSystem.Services.Interfaces;

namespace AccessControlSystem.ConsoleApp.UI.Handlers;

public class VisitorHandler
{
        private readonly IAccessControlService _service;
        
        public VisitorHandler(IAccessControlService service)
        {
            _service = service;
        }
        
        public async Task RegisterVisitor()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ ПОСЕТИТЕЛЯ ===");
            
            Console.Write("ФИО посетителя: ");
            var fullName = Console.ReadLine();
            
            Console.Write("Организация: ");
            var organization = Console.ReadLine();
            
            Console.Write("Контактная информация: ");
            var contactInfo = Console.ReadLine();
            
            Console.Write("Цель визита: ");
            var purpose = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(organization))
            {
                Console.WriteLine("❌ ФИО и организация обязательны!");
                return;
            }
            
            try
            {
                var visitor = await _service.RegisterVisitorAsync(
                    fullName, organization, contactInfo ?? "", purpose ?? "");
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ Посетитель зарегистрирован!");
                Console.ResetColor();
                Console.WriteLine($"ID пропуска: {visitor.Id}");
                Console.WriteLine($"ФИО: {visitor.FullName}");
                Console.WriteLine($"Время въезда: {visitor.EntryTime:HH:mm:ss}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }
        
        public async Task MarkVisitorExit()
        {
            Console.Clear();
            Console.WriteLine("=== ОТМЕТКА ВЫХОДА ПОСЕТИТЕЛЯ ===");
            
            Console.Write("ID посетителя: ");
            if (!int.TryParse(Console.ReadLine(), out int visitorId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }
            
            try
            {
                var success = await _service.MarkVisitorExitAsync(visitorId);
                
                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n✅ Выход отмечен!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n⚠️ Посетитель не найден или уже вышел!");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }
    }