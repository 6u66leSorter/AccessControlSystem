using AccessControlSystem.Entities;

namespace AccessControlSystem.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        
        if (!context.Employees.Any())
        {
            context.Employees.AddRange(
                new Employee { FullName = "Иванов И.И.", CardNumber = "EMP001", IsActive = true },
                new Employee { FullName = "Петров П.П.", CardNumber = "EMP002", IsActive = false }
            );
                
            context.SaveChanges();
        }
    }
}