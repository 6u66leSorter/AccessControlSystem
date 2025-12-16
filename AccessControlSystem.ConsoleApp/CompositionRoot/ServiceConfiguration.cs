using AccessControlSystem.ConsoleApp.UI;
using AccessControlSystem.ConsoleApp.UI.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AccessControlSystem.Data;
using AccessControlSystem.Repositories.Implementations;
using AccessControlSystem.Repositories.Interfaces;
using AccessControlSystem.Services.Implementations;
using AccessControlSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.ConsoleApp.CompositionRoot;

public static class ServiceConfiguration
{
    public static IServiceProvider ConfigureServices()
    {
        var configuration = BuildConfiguration();
            
        var services = new ServiceCollection();
        ConfigureLogging(services);
        ConfigureDatabase(services, configuration);
        ConfigureRepositories(services);
        ConfigureServicesLayer(services);
            
        return services.BuildServiceProvider();
    }
        
    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
    }
        
    private static void ConfigureLogging(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
    }
        
    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
        
    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVisitorRepository, VisitorRepository>();
        services.AddScoped<IAccessLogRepository, AccessLogRepository>();
    }
        
    private static void ConfigureServicesLayer(IServiceCollection services)
    {
        services.AddScoped<IAccessControlService, AccessControlService>();
        
        services.AddScoped<MenuService>();
        services.AddScoped<EmployeeHandler>();
        services.AddScoped<VehicleHandler>();
        services.AddScoped<VisitorHandler>();
        services.AddScoped<LogHandler>();
    }
}