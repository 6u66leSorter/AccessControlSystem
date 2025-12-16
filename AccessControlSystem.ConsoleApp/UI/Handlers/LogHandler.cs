using AccessControlSystem.Entities;
using AccessControlSystem.Services;
using AccessControlSystem.Services.Interfaces;

namespace AccessControlSystem.ConsoleApp.UI.Handlers
{
    public class LogHandler
    {
        private readonly IAccessControlService _service;
        
        public LogHandler(IAccessControlService service)
        {
            _service = service;
        }
        
        public async Task ShowTodayLogs()
        {
            Console.Clear();
            Console.WriteLine("=== ЖУРНАЛ СОБЫТИЙ ЗА СЕГОДНЯ ===\n");
            
            try
            {
                var logs = await _service.GetTodayAccessLogsAsync();
                
                if (!logs.Any())
                {
                    Console.WriteLine("Сегодня еще не было событий");
                    return;
                }
                
                Console.WriteLine("┌────┬────────────┬──────────┬────────────────────┬────────────────────┐");
                Console.WriteLine("│ ID │ Тип        │ Вход/Вых │ Время              │ Информация         │");
                Console.WriteLine("├────┼────────────┼──────────┼────────────────────┼────────────────────┤");
                
                foreach (var log in logs.OrderByDescending(l => l.AccessTime).Take(20))
                {
                    var type = GetEntityTypeShort(log.EntityType);
                    var direction = log.IsEntry ? "Вход" : "Выход";
                    var time = log.AccessTime.ToString("HH:mm:ss");
                    var info = GetLogInfo(log);
                    
                    Console.WriteLine($"│ {log.Id,-2} │ {type,-10} │ {direction,-8} │ {time,-18} │ {info,-18} │");
                }
                
                Console.WriteLine("└────┴────────────┴──────────┴────────────────────┴────────────────────┘");
                
                // Статистика
                var stats = logs
                    .GroupBy(l => l.EntityType)
                    .Select(g => new
                    {
                        Type = g.Key,
                        Count = g.Count(),
                        Entries = g.Count(l => l.IsEntry),
                        Exits = g.Count(l => !l.IsEntry)
                    });
                
                Console.WriteLine($"\n📊 Статистика за сегодня:");
                Console.WriteLine($"Всего событий: {logs.Count()}");
                
                foreach (var stat in stats)
                {
                    Console.WriteLine($"  {GetEntityTypeFull(stat.Type)}: {stat.Count} ({stat.Entries} входов, {stat.Exits} выходов)");
                }
                
                if (logs.Count() > 20)
                {
                    Console.WriteLine($"\n... и еще {logs.Count() - 20} записей");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Ошибка при загрузке журнала: {ex.Message}");
                Console.ResetColor();
            }
        }
        /*
        public async Task ShowLogsByDate()
        {
            Console.Clear();
            Console.WriteLine("=== ЖУРНАЛ СОБЫТИЙ ПО ДАТЕ ===");
            
            Console.Write("Введите дату (дд.мм.гггг) или нажмите Enter для сегодня: ");
            var dateInput = Console.ReadLine();
            
            DateTime targetDate;
            if (string.IsNullOrWhiteSpace(dateInput))
            {
                targetDate = DateTime.Today;
            }
            else
            {
                if (!DateTime.TryParse(dateInput, out targetDate))
                {
                    Console.WriteLine("❌ Неверный формат даты!");
                    return;
                }
            }
            
            Console.WriteLine($"\nЗагрузка событий за {targetDate:dd.MM.yyyy}...");
            
            try
            {
                // Нужно будет добавить метод в сервис
                // var logs = await _service.GetAccessLogsByDateAsync(targetDate);
                // Покажем заглушку
                Console.WriteLine("Функция поиска по дате в разработке...");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }
        
        public async Task ShowLogsByEntity()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК СОБЫТИЙ ПО СУЩНОСТИ ===");
            
            Console.WriteLine("Выберите тип сущности:");
            Console.WriteLine("1 - Сотрудник");
            Console.WriteLine("2 - Транспорт");
            Console.WriteLine("3 - Посетитель");
            Console.Write("Выберите: ");
            
            var entityChoice = Console.ReadLine();
            EntityType entityType;
            
            switch (entityChoice)
            {
                case "1":
                    entityType = EntityType.Employee;
                    break;
                case "2":
                    entityType = EntityType.Vehicle;
                    break;
                case "3":
                    entityType = EntityType.Visitor;
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    return;
            }
            
            Console.Write("Введите ID сущности: ");
            if (!int.TryParse(Console.ReadLine(), out int entityId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }
            
            try
            {
                Console.WriteLine($"\nЗагрузка событий для {GetEntityTypeFull(entityType)} ID: {entityId}...");
                
                // Для сотрудников используем существующий метод
                if (entityType == EntityType.Employee)
                {
                    var logs = await _service.GetEmployeeAccessLogsAsync(entityId, 50);
                    DisplayLogs(logs, $"События сотрудника ID: {entityId}");
                }
                else
                {
                    Console.WriteLine("Функция для транспорта и посетителей в разработке...");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }
        
        private void DisplayLogs(IEnumerable<AccessLog> logs, string title)
        {
            if (!logs.Any())
            {
                Console.WriteLine("События не найдены");
                return;
            }
            
            Console.WriteLine($"\n{title}");
            Console.WriteLine("┌────┬──────────┬────────────────────┐");
            Console.WriteLine("│ ID │ Вход/Вых │ Время              │");
            Console.WriteLine("├────┼──────────┼────────────────────┤");
            
            foreach (var log in logs.OrderByDescending(l => l.AccessTime))
            {
                var direction = log.IsEntry ? "Вход" : "Выход";
                var time = log.AccessTime.ToString("dd.MM HH:mm");
                
                Console.WriteLine($"│ {log.Id,-2} │ {direction,-8} │ {time,-18} │");
            }
            
            Console.WriteLine("└────┴──────────┴────────────────────┘");
        }
        */
        private string GetEntityTypeShort(EntityType entityType)
        {
            return entityType switch
            {
                EntityType.Employee => "Сотрудник",
                EntityType.Vehicle => "Транспорт",
                EntityType.Visitor => "Посетитель",
                _ => "Неизвестно"
            };
        }
        
        private string GetEntityTypeFull(EntityType entityType)
        {
            return entityType switch
            {
                EntityType.Employee => "сотрудника",
                EntityType.Vehicle => "транспорта",
                EntityType.Visitor => "посетителя",
                _ => "неизвестной сущности"
            };
        }

        private string GetLogInfo(AccessLog log)
        {
            // Простая реализация - можно расширить
            return log.EntityType switch
            {
                EntityType.Employee => $"{log.Employee?.CardNumber}",
                EntityType.Vehicle => $"{log.VehiclePass?.VehicleNumber}",
                EntityType.Visitor => $"{log.Visitor?.FullName}",
                _ => "Неизвестно"
            };
        }
    }
}