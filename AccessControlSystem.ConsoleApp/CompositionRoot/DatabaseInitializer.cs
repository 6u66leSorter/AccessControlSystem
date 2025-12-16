using AccessControlSystem.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControlSystem.ConsoleApp.CompositionRoot;

public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
}