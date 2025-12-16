using AccessControlSystem.Entities;

namespace AccessControlSystem.ConsoleApp.UI
{
    public static class ConsoleHelpers
    {
        public static void PrintVehiclesTable(IEnumerable<VehiclePass> vehicles)
        {
            Console.WriteLine("┌────┬─────────────────┬──────────────────────┬────────────────────┬──────────────┐");
            Console.WriteLine("│ ID │ Гос. номер      │ Водитель             │ Организация        │ Время въезда │");
            Console.WriteLine("├────┼─────────────────┼──────────────────────┼────────────────────┼──────────────┤");
            
            foreach (var vehicle in vehicles)
            {
                var entryTime = vehicle.EntryTime.ToString("dd.MM HH:mm");
                Console.WriteLine($"│ {vehicle.Id,-2} │ {vehicle.VehicleNumber,-15} │ {vehicle.DriverName,-20} │ {vehicle.Organization,-18} │ {entryTime,-12} │");
            }
            
            Console.WriteLine("└────┴─────────────────┴──────────────────────┴────────────────────┴──────────────┘");
        }
        
        public static void PrintVisitorsTable(IEnumerable<Visitor> visitors)
        {
            Console.WriteLine("┌────┬──────────────────────┬────────────────────┬────────────────────┬──────────────┐");
            Console.WriteLine("│ ID │ ФИО                  │ Организация        │ Контакт            │ Время въезда │");
            Console.WriteLine("├────┼──────────────────────┼────────────────────┼────────────────────┼──────────────┤");
            
            foreach (var visitor in visitors)
            {
                var entryTime = visitor.EntryTime.ToString("dd.MM HH:mm");
                Console.WriteLine($"│ {visitor.Id,-2} │ {visitor.FullName,-20} │ {visitor.Organization,-18} │ {visitor.ContactPerson,-18} │ {entryTime,-12} │");
            }
            
            Console.WriteLine("└────┴──────────────────────┴────────────────────┴────────────────────┴──────────────┘");
        }
        
        public static void PrintAccessLogsTable(IEnumerable<AccessLog> logs)
        {
            Console.WriteLine("┌────┬────────────┬──────────┬────────────────────┬────────────────────┐");
            Console.WriteLine("│ ID │ Тип        │ Вход/Вых │      Время         │ Информация         │");
            Console.WriteLine("├────┼────────────┼──────────┼────────────────────┼────────────────────┤");
            
            foreach (var log in logs.Take(20))
            {
                var type = log.EntityType.ToString()[..3];
                var direction = log.IsEntry ? "Вход" : "Выход";
                var time = log.AccessTime.ToString("HH:mm:ss");
                var info = GetLogInfo(log);
                
                Console.WriteLine($"│ {log.Id,-2} │ {type,-10} │ {direction,-8} │ {time,-18} │ {info,-18} │");
            }
            
            Console.WriteLine("└────┴────────────┴──────────┴────────────────────┴────────────────────┘");
        }
        
        private static string GetLogInfo(AccessLog log)
        {
            return log.EntityType switch
            {
                EntityType.Employee => log.Employee?.FullName ?? "Сотрудник",
                EntityType.Vehicle => log.VehiclePass?.VehicleNumber ?? "Транспорт",
                EntityType.Visitor => log.Visitor?.FullName ?? "Посетитель",
                _ => "Неизвестно"
            };
        }
    }
}