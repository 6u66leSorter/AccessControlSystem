using AccessControlSystem.ConsoleApp.CompositionRoot;
using AccessControlSystem.ConsoleApp.UI;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControlSystem.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🚀 Запуск системы контроля доступа КПП");
            
            var serviceProvider = ServiceConfiguration.ConfigureServices();
            DatabaseInitializer.Initialize(serviceProvider);
            
            using var scope = serviceProvider.CreateScope();
            var menuService = scope.ServiceProvider.GetRequiredService<MenuService>();
            await menuService.RunAsync();
        }
    }
}