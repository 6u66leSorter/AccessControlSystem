namespace AccessControlSystem.Entities;

public class AccessLog
{
    public int Id { get; set; }
    public EntityType EntityType { get; set; }
    public int EntityId { get; set; }
    
    public DateTime AccessTime { get; set; }
    public bool IsEntry { get; set; }
    public string? Reason { get; set; }
    
    public int? EmployeeId { get; set; }
    public int? VehiclePassId { get; set; }
    public int? VisitorId { get; set; }
    
    public virtual Employee? Employee { get; set; }
    public virtual VehiclePass? VehiclePass { get; set; }
    public virtual Visitor? Visitor { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum EntityType
{
    Employee,
    Vehicle,
    Visitor
}