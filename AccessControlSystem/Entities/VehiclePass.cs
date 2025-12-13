namespace AccessControlSystem.Entities;

public class VehiclePass
{
    public int Id { get; set; }
    public string VehicleNumber { get; set; } = string.Empty; // А123ВС77
    public string DriverName { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public bool IsInternal { get; set; } // true = свой транспорт
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public bool IsCompleted => ExitTime.HasValue;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}