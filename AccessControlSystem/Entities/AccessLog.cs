namespace AccessControlSystem.Entities;

public class AccessLog
{
    public int Id { get; set; }
    public EntityType EntityType { get; set; }
    public int EntityId { get; set; }
    public DateTime AccessTime { get; set; }
    public bool IsEntry { get; set; } // true = вход/въезд, false = выход/выезд
    public string CheckpointNumber { get; set; } = string.Empty; // КПП-1, КПП-2
    public string? Reason { get; set; } // Причина отказа
        
    // Внешние ключи
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
        
    public int? VehiclePassId { get; set; }
    public VehiclePass? VehiclePass { get; set; }
        
    public int? VisitorId { get; set; }
    public Visitor? Visitor { get; set; }
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum EntityType
{
    Employee,
    Vehicle,
    Visitor
}